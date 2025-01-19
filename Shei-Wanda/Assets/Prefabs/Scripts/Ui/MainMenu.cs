using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine;
using System;
//using WSMulti.Gameplay.Player;
//using CleanLaboratory.Gameplay;

namespace CleanLaboratory.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField PseudoField, IPField, PortField;

        /// <summary>
        /// Starts the host using the given connection data.
        /// </summary>
        public void StartHost()
        {
            SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartHost();
            if (result)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Level 0", UnityEngine.SceneManagement.LoadSceneMode.Single);
                Debug.Log("Hôte démarré avec succès.");
                // AssignPlayerInformation();
            }
            else
            {
                Debug.LogError("Échec du démarrage de l'hôte.");
            }

        }

        /// <summary>
        /// Starts the Client using the given connection data.
        /// </summary>
        public void StartClient()
        {
            SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartClient();
            if (result)
            {
                Debug.Log("Client connecté avec succès.");
                //AssignPlayerInformation();
            }
            else
            {
                Debug.LogError("Échec de la connexion du client.");
            }
        }

        /*public void AssignPlayerInformation()
        {
            PlayerData pm = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerData>();
            var pseudo = PseudoField.text;
            if (pseudo == "")
            {
                pseudo = RandomString(5);
            }
            pm.PlayerPseudo = pseudo;
        }*/

        public static string RandomString(int length)
        {
            System.Random rnd = new System.Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Use sanitized IP and Port to set up the connection.
        /// </summary>
        void SetUtpConnectionData()
        {
            var sanitizedIPText = SanitizeAlphaNumeric(IPField.text);
            var sanitizedPortText = SanitizeAlphaNumeric(PortField.text);
            if (IPField.text == "")
            {
                sanitizedIPText = "127.0.0.1";
            }
            if (PortField.text == "")
            {
                sanitizedPortText = "4242";
            }

            ushort.TryParse(sanitizedPortText, out var port);
            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(sanitizedIPText, port);
        }

        /// <summary>
        /// Sanitize user port InputField box allowing only alphanumerics and '.'
        /// </summary>
        /// <param name="dirtyString"> string to sanitize. </param>
        /// <returns> Sanitized text string. </returns>
        static string SanitizeAlphaNumeric(string dirtyString)
        {
            return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
        }
    }
}
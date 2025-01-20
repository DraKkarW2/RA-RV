using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityChan
{
    public class FaceUpdate : MonoBehaviour
    {
        public AnimationClip[] animations;
        Animator anim;
        public float delayWeight;
        public bool isKeepFace = false;

        // Référence à l'action d'entrée pour les boutons
        public InputActionProperty selectAction;  // Action pour sélectionner (ex : bouton A ou X sur le contrôleur)

        void Start()
        {
            anim = GetComponent<Animator>();

            // Assurez-vous que l'action est activée au démarrage
            selectAction.action.Enable();
        }

        void Update()
        {
            // Vérifier si l'action d'entrée est pressée
            if (selectAction.action.ReadValue<float>() > 0) // La valeur peut être 1 quand le bouton est pressé
            {
                isKeepFace = true;
                anim.CrossFade("default@unitychan", 0);
            }
            else if (!isKeepFace)
            {
                isKeepFace = false;
                anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0, delayWeight));
            }
        }

        public void OnCallChangeFace(string str)
        {
            int ichecked = 0;
            foreach (var animation in animations)
            {
                if (str == animation.name)
                {
                    ChangeFace(str);
                    break;
                }
                else if (ichecked <= animations.Length)
                {
                    ichecked++;
                }
                else
                {
                    str = "default@unitychan"; // Animation par défaut
                    ChangeFace(str);
                }
            }
        }

        void ChangeFace(string str)
        {
            isKeepFace = true;
            anim.CrossFade(str, 0);
        }
    }
}
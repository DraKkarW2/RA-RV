using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        
    }
    public void QuitApplication()
    {
        Debug.Log("Fermeture de l'application...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Pour arrêter l’éditeur
#endif
    }
}

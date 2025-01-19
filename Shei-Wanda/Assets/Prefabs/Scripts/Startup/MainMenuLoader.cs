using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    void Start()
    {
        Debug.Log("ici fesse");
        SceneManager.LoadScene("MainMenu");
    }
}

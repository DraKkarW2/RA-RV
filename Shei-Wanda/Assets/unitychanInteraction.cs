using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanInteraction : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand")) 
        {
            Debug.Log("Unity-chan touchée !");
        }
    }
}
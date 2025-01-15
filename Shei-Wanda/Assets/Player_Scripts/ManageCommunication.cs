using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCommunication : MonoBehaviour
{
    public bool canCommunicate = true; 

    public void UpdateCommunicationStatus(float sanity)
    {
        // Modifier potentiellement la valeur de sanity
        if (sanity <= 20f)
        {
            canCommunicate = false;
        }
        else
        {
            canCommunicate = true;
        }
    }
}

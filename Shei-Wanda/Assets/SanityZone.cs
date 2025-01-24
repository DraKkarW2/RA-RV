using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityZone : MonoBehaviour
{
    public bool isDarkZone = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerDefault"))
        {
            SanityManager sanityManager = other.GetComponent<SanityManager>();
            if (sanityManager != null)
            {
                sanityManager.isInDarkness = isDarkZone;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("playerDefault"))
        {
            SanityManager sanityManager = other.GetComponent<SanityManager>();
            if (sanityManager != null)
            {
                sanityManager.isInDarkness = false; 
            }
        }
    }
}

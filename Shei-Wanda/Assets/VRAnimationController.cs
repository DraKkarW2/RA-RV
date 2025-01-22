using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimationController : MonoBehaviour
{
    public Animator animator; // Gestion des animations
    public Transform leftHand; // Main gauche VR
    public Transform rightHand; // Main droite VR
    public Transform playerTransform; // Référence au Player

    private float speed;

    void Update()
    {
        // Synchronisation des animations
        if (leftHand != null && rightHand != null)
        {
            animator.SetFloat("HandDistance", Vector3.Distance(leftHand.position, rightHand.position));
        }

        // Calculer la vitesse en fonction des déplacements du Player
        Vector3 playerVelocity = playerTransform.GetComponent<CharacterController>().velocity;
        speed = playerVelocity.magnitude;

        // Appliquer la vitesse à l'Animator
        animator.SetFloat("Speed", speed);

        // Synchroniser UnityChan avec le Player
        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
    }
}
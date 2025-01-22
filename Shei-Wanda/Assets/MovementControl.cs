using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    public CharacterController characterController; // Référence au Character Controller
    public float moveSpeed = 2.0f; // Vitesse de déplacement
    public float rotationSpeed = 10.0f; // Vitesse de rotation
    public Animator animator; // Référence à l'Animator

    void Start()
    {
        // Associe automatiquement le Character Controller si ce n'est pas fait
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Récupère les inputs de déplacement (à ajuster si tu es en VR)
        float horizontal = Input.GetAxis("Horizontal"); // Pour gauche/droite
        float vertical = Input.GetAxis("Vertical"); // Pour avancer/reculer

        // Crée un vecteur de mouvement
        Vector3 move = new Vector3(horizontal, 0, vertical);
        move = transform.TransformDirection(move); // Oriente le mouvement selon la rotation du personnage

        // Applique le mouvement au Character Controller
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Détecte si le personnage bouge et envoie la valeur à l'Animator
        float speed = move.magnitude; // Calcul de la vitesse
        animator.SetFloat("Speed", speed); // Met à jour le paramètre "Speed"

        // Gère la rotation du personnage
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


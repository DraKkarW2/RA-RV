using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRIK : MonoBehaviour
{
    public Transform leftHandTarget; // Le contrôleur gauche
    public Transform rightHandTarget; // Le contrôleur droit
    public Transform leftHand; // L'os "Hand_l" du modèle
    public Transform rightHand; // L'os "Hand_r" du modèle
    public Animator animator;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            // Assurez-vous que l'IK est activé sur les mains
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

            // Fixer la position et la rotation des mains sur les contrôleurs
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimationController : MonoBehaviour
{
    public Animator animator;

    public Transform leftHand;
    public Transform rightHand;

    public CharacterController characterController; 
    public float moveSpeed = 1.0f; 

    private float speed;

    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        speed = Mathf.Max(leftHand.position.magnitude, rightHand.position.magnitude);
        animator.SetFloat("Speed", speed);

        if (leftHand != null && rightHand != null)
        {
            animator.SetFloat("HandDistance", Vector3.Distance(leftHand.position, rightHand.position));
        }

        Vector3 move = transform.forward * speed * moveSpeed * Time.deltaTime;
        characterController.Move(move);
    }
}

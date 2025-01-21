using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimationController : MonoBehaviour
{
    public Animator animator;
    public Transform leftHand;
    public Transform rightHand;

    private float speed;

    void Update()
    {
        speed = Mathf.Max(leftHand.position.magnitude, rightHand.position.magnitude);
        animator.SetFloat("Speed", speed);  

        if (leftHand != null)
        {
            animator.SetFloat("HandDistance", Vector3.Distance(leftHand.position, rightHand.position));
        }
    }
}

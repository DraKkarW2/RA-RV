using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;

    public float rotationX = 0f;

    void Start()
    {
        
    }

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertival = Input.GetAxis("vertical");

        Vector3 direction = transform.right * horizontal + transform.forward * vertival;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        bool isMoving = Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0;
        animator.SetBool("isMoving", isMoving);


    }
}

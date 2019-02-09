﻿using UnityEngine;

public class PlayerControllerSimpleWithSlope : MonoBehaviour
{
    public float velocity = 5;
    public float turnSpeed = 10;

    private Vector2 input;
    private float angle;
    private Quaternion targetRotation;
    private Transform levelCamera;

    public float jumpForce = 1;
    public LayerMask ground;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private RaycastHit hit;
    int layerMask = 1 << 8;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        levelCamera = Camera.main.transform;
    }


    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (Mathf.Abs(input.x) == 0 && Mathf.Abs(input.y) == 0)
        {
            return;
        }

        CalculateDirection();
        Rotate();
        Move();
    }

    private void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += levelCamera.eulerAngles.y;
    }

    private void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Controller_A") == 1 && Grounded())
        {
            Debug.Log("Pressed A");
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }

        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        float rotationAngle = Vector3.Angle(hit.normal, transform.up);
Debug.Log("Rotation angle: " + rotationAngle);
Debug.Log("Current rotation: " + transform.rotation);
        transform.Rotate(new Vector3(rotationAngle, 0, 0));
Debug.Log("New rotation: " + transform.rotation);
    }

    private bool Grounded()
    {
        return Physics.CheckCapsule(capsuleCollider.bounds.center,
                                    new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.center.z),
                                    capsuleCollider.radius,
                                    ground);
    }
}

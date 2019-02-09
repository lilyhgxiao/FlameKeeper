using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int forceMagnitude = 10;
    public int jumpSpeed = 5;

    private bool blocksDestroyed = false;
    private Vector3 originalPosition;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        this.originalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
			Vector3 vel = body.velocity;
			vel.y = jumpSpeed;
			body.velocity = vel;
        }
    }

    void FixedUpdate()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 vel = body.velocity;
        vel.x = xMovement * forceMagnitude;
        vel.z = zMovement * forceMagnitude;
        body.velocity = vel;
    }

    public void SetSpeed(int newSpeed)
    {
        forceMagnitude = newSpeed;
    }
}

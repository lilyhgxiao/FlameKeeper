using UnityEngine;

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

        CalculateDirection();
        Rotate();
        Move();

        Physics.Raycast(this.transform.position, Vector3.down, out hit);
        //transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
    }

    private bool Grounded()
    {
        return Physics.CheckCapsule(capsuleCollider.bounds.center,
                                    new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.center.z),
                                    capsuleCollider.radius,
                                    ground);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    private void OnCollisionStay(Collision collision)
    {
        rb.angularVelocity = new Vector3(0, 0, 0);
    }
}

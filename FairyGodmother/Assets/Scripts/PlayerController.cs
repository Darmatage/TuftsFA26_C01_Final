using UnityEngine;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    private CinemachineCamera cameraFreeLook;
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float rotationSpeed = 15f;
    public float smoothTime = 0.1f;
    float dashVelocity = 1f;

    Transform mainCam;

    const float ZeroF = 0f;
    Vector3 movement;
    float currentSpeed;
    float velocity;
    private Rigidbody rb;
    private bool isGrounded = true;

    void Awake()
    {
        mainCam = Camera.main.transform;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		cameraFreeLook = GameObject.FindWithTag("Freelook").GetComponent<CinemachineCamera>();
    }

    

    void Update()
    {
        //read movement input
        movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        //Jumping when alreayd on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        //Attacking - WRITE CODE FOR IT
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }


    void FixedUpdate()
    {
        HandleMovement();
    }

    //defines camera movement and calls other functions for movement
    public void HandleMovement()
    {
        //define directions
        Vector3 camForward = mainCam.forward;
        Vector3 camRight = mainCam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        //main thing to always update the direction of camera
        Vector3 adjustedDirection = (camForward * movement.z + camRight * movement.x).normalized;

        //if there is input, call other functions
        //otherwise set velocity to 0
        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }

    //defines speed and velocity
    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        //left shift should define speed
        //DOUBLE CHECK IF ITS ACTUALLY HAPPENING
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        Vector3 velocity = adjustedDirection * (speed * dashVelocity);

        //sets velocity
        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    //rotation based on direction
    void HandleRotation(Vector3 adjustedDirection)
    {
        var targetRotation = Quaternion.LookRotation(adjustedDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }


    // void SmoothSpeed(float value)
    // {
    //     currentSpeed = Mathf.SmoothDamp(
    //         currentSpeed,
    //         value,
    //         ref velocity,
    //         smoothTime
    //     );
    // }

    void Attack()
        {
            Debug.Log("Player attacks!");
            //ADD STH HERE
        }

    //check if standing on the ground for jump purposes
    private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
    }
}

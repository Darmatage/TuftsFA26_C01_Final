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

    public float airBoostSpeed = 14f;
    public float airBoostDuration = 0.25f;

    bool hasAirBoosted;
    float airBoostTimer;
    Vector3 airBoostDirection;

    float dashVelocity = 1f;

    Transform mainCam;

    const float ZeroF = 0f;
    Vector3 movement;
    float currentSpeed;
    float velocity;

    private Rigidbody rb;
    private bool isGrounded = true;

    // Moving platform variables
    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    private Quaternion lastPlatformRotation;

    void Awake()
    {
        mainCam = Camera.main.transform;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cameraFreeLook = GameObject.FindWithTag("Freelook")
            .GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        // Read movement input
        movement = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isGrounded = false;

            // Stop following platform when jumping
            currentPlatform = null;
        }

        // Attack
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded && !hasAirBoosted)
        {
            hasAirBoosted = true;
            airBoostTimer = airBoostDuration;
            airBoostDirection = GetMoveDirection();
        }
    }

    void FixedUpdate()
    {
        HandlePlatformMovement();
        HandleMovement();
    }

    // Apply platform movement to player
    void HandlePlatformMovement()
    {
        if (currentPlatform == null)
            return;

        Vector3 platformMovement =
            currentPlatform.position - lastPlatformPosition;

        Quaternion platformRotation =
            currentPlatform.rotation * Quaternion.Inverse(lastPlatformRotation);

        // Move player with platform
        Vector3 relativePosition =
            rb.position - lastPlatformPosition;

        Vector3 rotatedPosition =
            platformRotation * relativePosition;

        Vector3 newPosition =
            lastPlatformPosition + rotatedPosition + platformMovement;

        rb.MovePosition(newPosition);

        // Update previous platform transform
        lastPlatformPosition = currentPlatform.position;
        lastPlatformRotation = currentPlatform.rotation;
    }

    // Detect movement input
    public void HandleMovement()
    {

        if (airBoostTimer > 0f)
        {
            airBoostTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(
                airBoostDirection.x * airBoostSpeed,
                rb.linearVelocity.y,
                airBoostDirection.z * airBoostSpeed
            );
            return;
        }

        Vector3 camForward = mainCam.forward;
        Vector3 camRight = mainCam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 adjustedDirection =
            (camForward * movement.z + camRight * movement.x).normalized;

        if (adjustedDirection.magnitude > ZeroF)
        {
            HandleRotation(adjustedDirection);
            HandleHorizontalMovement(adjustedDirection);
        }
        else
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );
        }
    }

    // Horizontal movement
    void HandleHorizontalMovement(Vector3 adjustedDirection)
    {
        float speed = Input.GetKey(KeyCode.LeftShift)
            ? runSpeed
            : moveSpeed;

        Vector3 newVelocity =
            adjustedDirection * (speed * dashVelocity);

        rb.linearVelocity = new Vector3(
            newVelocity.x,
            rb.linearVelocity.y,
            newVelocity.z
        );
    }

    Vector3 GetMoveDirection()
    {
        Vector3 camForward = mainCam.forward;
        Vector3 camRight = mainCam.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 dir = (camForward * movement.z + camRight * movement.x).normalized;

        // No input held: boost the way the player is facing
        return dir.magnitude > 0f ? dir : transform.forward;
    }

    // Player rotation
    void HandleRotation(Vector3 adjustedDirection)
    {
        var targetRotation =
            Quaternion.LookRotation(adjustedDirection);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }

    void Attack()
    {
        Debug.Log("Player attacks!");
    }

    // Detect ground and moving platforms
    private void OnCollisionEnter(Collision collision)
    {
        CheckPlatform(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckPlatform(collision);
    }

    void CheckPlatform(Collision collision)
    {
        if (rb.linearVelocity.y > 0.1f)
            return;
            
        foreach (ContactPoint contact in collision.contacts)
        {
            // Only detect surfaces underneath the player
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                hasAirBoosted = false;

                if (collision.gameObject.CompareTag("Ground"))
                {
                    if (currentPlatform != collision.transform)
                    {
                        currentPlatform = collision.transform;

                        lastPlatformPosition = currentPlatform.position;
                        lastPlatformRotation = currentPlatform.rotation;
                    }
                }

                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (currentPlatform == collision.transform)
        {
            currentPlatform = null;
        }
    }
}
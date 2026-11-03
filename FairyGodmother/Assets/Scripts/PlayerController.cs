using UnityEngine;
using Unity.Cinemachine;

public class PlayerController : MonoBehaviour
{
    public CinemachineCamera cameraFreeLook;
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
        // cameraFreeLook.Follow = transform;
        // cameraFreeLook.LookAt = transform;
        // cameraFreeLook.OnTargetObjectWarped(transform, transform.position - cameraFreeLook.transform.position - Vector3.forward);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    

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
    }

    // Attack
    if (Input.GetMouseButtonDown(0))
    {
        Attack();
    }
}


void FixedUpdate()
{
    HandleMovement();
}


public void HandleMovement()
{
    // Convert input into camera-relative direction on the ground plane
    Vector3 camForward = mainCam.forward;
    Vector3 camRight = mainCam.right;
    camForward.y = 0f;
    camRight.y = 0f;
    camForward.Normalize();
    camRight.Normalize();

    Vector3 adjustedDirection = (camForward * movement.z + camRight * movement.x).normalized;

    if (adjustedDirection.magnitude > ZeroF)
    {
        //Debug.Log($"dir: {adjustedDirection}  vel: {rb.linearVelocity}  camFwd: {mainCam.forward}");
        HandleRotation(adjustedDirection);
        HandleHorizontalMovement(adjustedDirection);
    }
    else
    {
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }
}


void HandleHorizontalMovement(Vector3 adjustedDirection)
{
    float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

    Vector3 velocity = adjustedDirection * (speed * dashVelocity);

    rb.linearVelocity = new Vector3(
        velocity.x,
        rb.linearVelocity.y,
        velocity.z
    );
}

void HandleRotation(Vector3 adjustedDirection)
{
    var targetRotation = Quaternion.LookRotation(adjustedDirection);

    transform.rotation = Quaternion.Slerp(
        transform.rotation,
        targetRotation,
        rotationSpeed * Time.fixedDeltaTime
    );
}

void SmoothSpeed(float value)
{
    currentSpeed = Mathf.SmoothDamp(
        currentSpeed,
        value,
        ref velocity,
        smoothTime
    );
}

void Attack()
    {
        Debug.Log("Player attacks!");
        // Add your attack animation or combat logic here.
    }

private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

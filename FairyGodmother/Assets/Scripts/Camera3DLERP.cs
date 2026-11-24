using UnityEngine;

public class Camera3DLERP : MonoBehaviour
{
    private Transform target;
    public float smoother = 5f;
    public Vector3 offset = new Vector3(4, 2, 0); // Sets starting angle and distance

    [Header("Mouse Orbit Settings")]
    public float mouseSensitivity = 3f;
    public float minPitch = -20f;
    public float maxPitch = 75f;
    public bool holdRightClickToOrbit = false; // Check this if you want a visible cursor for UI

    [Header("Scroll Zoom Settings")]
    public float zoomSpeed = 5f;
    public float zoomSmoothing = 10f;
    public float minDistance = 2f;
    public float maxDistance = 12f;

    private float yaw;
    private float pitch;
    private float currentDistance;
    private float targetDistance;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.GetComponent<Transform>();
        }

        // Convert your initial offset into distance and starting orbit angles
        currentDistance = Mathf.Clamp(offset.magnitude, minDistance, maxDistance);
        targetDistance = currentDistance;

        if (offset.magnitude > 0.001f)
        {
            Vector3 initialAngles = Quaternion.LookRotation(-offset.normalized).eulerAngles;
            yaw = initialAngles.y;
            pitch = (initialAngles.x > 180f) ? initialAngles.x - 360f : initialAngles.x;
        }

        if (!holdRightClickToOrbit)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Handle Mouse Orbit using built-in Input Manager
        bool canOrbit = !holdRightClickToOrbit || Input.GetMouseButton(1);
        if (canOrbit)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * mouseSensitivity;
            pitch -= mouseY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // 2. Handle Scroll Wheel Zoom using built-in Input Manager
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.001f)
        {
            targetDistance -= scrollInput * zoomSpeed;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // Smoothly interpolate toward the target zoom distance
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, zoomSmoothing * Time.deltaTime);

        // Calculate the rotated and zoomed offset around the player
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 rotatedOffset = rotation * new Vector3(0f, 0f, -currentDistance);

        Vector3 newPos = target.position + rotatedOffset;
        Vector3 smPos = Vector3.Lerp(transform.position, newPos, smoother * Time.deltaTime);
        transform.position = smPos;

        transform.LookAt(target);
    }
}

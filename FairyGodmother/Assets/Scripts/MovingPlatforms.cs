using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Waypoints & Speed")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;
    public float waitTime = 0.5f; // Pause duration at each end

    private Vector3 targetPos;
    private float waitTimer;

    void Start()
    {
        if (pointA != null)
        {
            transform.position = pointA.position;
        }
        if (pointB != null)
        {
            targetPos = pointB.position;
        }
    }

    void FixedUpdate()
    {
        if (pointA == null || pointB == null) return;

        // Pause briefly when reaching a waypoint
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        // Move smoothly toward the current target waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);

        // Switch target once the platform arrives
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            targetPos = (targetPos == pointB.position) ? pointA.position : pointB.position;
            waitTimer = waitTime;
        }
    }

    // Attach the Player to the platform so they move with it
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    // Detach the Player when they jump or walk off
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

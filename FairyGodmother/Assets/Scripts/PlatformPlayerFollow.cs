
using UnityEngine;

public class PlatformPlayerFollow : MonoBehaviour
{
    private Transform currentPlatform;
    private bool wasOnPlatform = false;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Only attach when touching the TOP of a platform
        if (hit.normal.y > 0.5f)
        {
            currentPlatform = hit.collider.transform;
        }
    }

    void LateUpdate()
    {
        if (currentPlatform != null)
        {
            // Make player follow the platform
            transform.SetParent(currentPlatform, true);
            wasOnPlatform = true;
        }
        else if (wasOnPlatform)
        {
            // Detach when player leaves the platform
            transform.SetParent(null, true);
            wasOnPlatform = false;
        }

        currentPlatform = null;
    }
}


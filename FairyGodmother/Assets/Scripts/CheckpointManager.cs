using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    
    public Transform player;

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private void Awake()
    {
        Debug.Log($"[CheckpointManager] Player ref = '{player.name}', start position saved = {respawnPosition}", this);
        Instance = this;

        respawnPosition = player.position;
        respawnRotation = player.rotation;
    }

    public void SetCheckpoint(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRotation = rotation;
        Debug.Log($"[CheckpointManager] Checkpoint saved at {position}");
    }

    public void Respawn()
    {
        Debug.Log($"[CheckpointManager] Respawning player at {respawnPosition}");
        if (player.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;  
            rb.angularVelocity = Vector3.zero;

            rb.position = respawnPosition;
            rb.rotation = respawnRotation;
        }

        
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.SetPositionAndRotation(respawnPosition, respawnRotation);
        Physics.SyncTransforms();

        if (cc != null) cc.enabled = true;
    }
}
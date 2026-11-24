using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public ParticleSystem reachedVfx;

    //public AudioSource reachedSound;

    private bool activated;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Checkpoint] '{name}' ENTERED by '{other.name}' (tag: {other.tag})", this);
        if (activated || !other.CompareTag("Player")) return;

        activated = true;

        Transform point = spawnPoint != null ? spawnPoint : transform;
        CheckpointManager.Instance.SetCheckpoint(point.position, point.rotation);

        if (reachedVfx != null) reachedVfx.Play();
        //if (reachedSound != null) reachedSound.Play();
    }
}

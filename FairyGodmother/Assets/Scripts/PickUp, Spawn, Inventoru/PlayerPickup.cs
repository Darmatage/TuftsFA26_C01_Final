using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private GameInventory inventory;
    [SerializeField] private AudioSource pickupSound;

    void Awake()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<GameInventory>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PickUp")) return;

        var pickup = other.GetComponent<ItemPickup>();
        if (pickup == null) return;

        if (inventory.AddItem(pickup.itemId, pickup.amount))
        {
            if (pickupSound != null) pickupSound.Play();
            Destroy(other.gameObject);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class StorageChest : MonoBehaviour
{
    [Header("Chest Settings")]
    public int maxSlots = 12;
    public List<InventorySlot> storedItems = new List<InventorySlot>();

    [Header("UI Reference")]
    public ChestUI chestUIManager;

    private bool playerInRange = false;
    private bool isOpen = false;

    void Update()
    {
        // Press E to open or close the chest when standing nearby
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen) CloseChest();
            else OpenChest();
        }
    }

    public void OpenChest()
    {
        isOpen = true;
        chestUIManager.OpenUI(this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseChest()
    {
        isOpen = false;
        chestUIManager.CloseUI();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool AddItem(ItemData itemToAdd, int amount = 1)
    {
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in storedItems)
            {
                if (slot.item == itemToAdd)
                {
                    slot.amount += amount;
                    return true;
                }
            }
        }

        if (storedItems.Count < maxSlots)
        {
            storedItems.Add(new InventorySlot(itemToAdd, amount));
            return true;
        }

        Debug.Log("Chest is full!");
        return false;
    }

    public void RemoveItem(InventorySlot slotToRemove)
    {
        storedItems.Remove(slotToRemove);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isOpen) CloseChest();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxSlots = 12;
    public List<InventorySlot> items = new List<InventorySlot>();

    public bool AddItem(ItemData itemToAdd, int amount = 1)
    {
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in items)
            {
                if (slot.item == itemToAdd)
                {
                    slot.amount += amount;
                    return true;
                }
            }
        }

        if (items.Count < maxSlots)
        {
            items.Add(new InventorySlot(itemToAdd, amount));
            return true;
        }

        Debug.Log("Player inventory is full!");
        return false;
    }

    public void RemoveItem(InventorySlot slotToRemove)
    {
        items.Remove(slotToRemove);
    }
}

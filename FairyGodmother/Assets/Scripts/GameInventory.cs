using UnityEngine;
using System.Collections.Generic;

public enum BuffType { Speed, Jump }
 

[System.Serializable]
public class ItemDefinition
{
    public string id;         
    public string displayName;
    public Sprite icon;
 
    [Header("Buff (right-click the slot to use)")]
    public bool usable;
    public BuffType buffType = BuffType.Speed;
    public float buffMultiplier = 1.5f;
    public float buffDuration = 10f; //0 is permanent
 
    [Header("Cooking")]
    public bool ingredient; //can be used for cooking
}

public class GameInventory : MonoBehaviour
{

    [Header("UI")]
    public GameObject InventoryMenu;
    public Transform slotContainer;
    public InventorySlotUI slotPrefab;
    public bool InvIsOpen = false;
    public bool openOnPickup = false;
    public bool showEmptySlots = false;
    
    [Header("Items in the game")]
    public List<ItemDefinition> itemDefinitions = new List<ItemDefinition>();
    static readonly Dictionary<string, int> counts = new Dictionary<string, int>();
    readonly Dictionary<string, ItemDefinition> definitionById = new Dictionary<string, ItemDefinition>();
    readonly Dictionary<string, InventorySlotUI> slotById = new Dictionary<string, InventorySlotUI>();
    PlayerBuffs playerBuffs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //build a list and create a slot for each ingredient
        //when scene is loaded should ideally restore the inventory
        foreach (var def in itemDefinitions)
        {
            if (string.IsNullOrEmpty(def.id))
            {
                Debug.LogWarning("An item definition has no id and was skipped.");
                continue;
            }
            if (definitionById.ContainsKey(def.id))
            {
                Debug.LogWarning($"Duplicate item id '{def.id}' skipped.");
                continue;
            }
 
            definitionById[def.id] = def;
 
            var slot = Instantiate(slotPrefab, slotContainer);
            slot.name = "Slot_" + def.id;
            slot.Setup(def, this);
            slotById[def.id] = slot;
        }
 
        InventoryMenu.SetActive(InvIsOpen);
        RefreshDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //double checks each ingredient and quantity
    public void RefreshDisplay()
    {
        foreach (var pair in slotById)
            pair.Value.Refresh(GetCount(pair.Key), showEmptySlots);
    }
 
    //helped for adding item
    public void InventoryAdd(string item)
    {
        AddItem(item, 1);
    }

    //manages creating an ingredient
    public bool AddItem(string item, int amount, bool openMenu = true)
    {
        if (!definitionById.ContainsKey(item))
        {
            Debug.Log($"Item '{item}' does not exist to be added");
            return false;
        }
 
        AddCount(item, amount);
        RefreshDisplay();
 
        if (openMenu && openOnPickup && !InvIsOpen)
            OpenCloseInventory();
        return true;
    }
 
    //helper to remove
    public void InventoryRemove(string item, int num)
    {
        if (!definitionById.ContainsKey(item))
        {
            Debug.Log($"Item '{item}' does not exist to be removed");
            return;
        }
 
        AddCount(item, -num);
        RefreshDisplay();
    }
 
    //remove if count less than amount
    public bool TryRemove(string item, int amount)
    {
        if (GetCount(item) < amount) return false;
        InventoryRemove(item, amount);
        return true;
    }

    //use an ingredient as a buff
    //NEED TO CHECK IF THIS EVEN WORKS
    public bool UseItem(string item)
    {
        if (!definitionById.TryGetValue(item, out var def) || !def.usable) return false;
        if (GetCount(item) <= 0) return false;
 
        if (playerBuffs == null)
            playerBuffs = FindFirstObjectByType<PlayerBuffs>();
        if (playerBuffs == null)
        {
            Debug.LogWarning("No PlayerBuffs component found in the scene. Add it to your player.");
            return false;
        }
 
        playerBuffs.Apply(def.buffType, def.buffMultiplier, def.buffDuration);
        InventoryRemove(item, 1);
        return true;
    }

    //get item info
    public ItemDefinition GetDefinition(string id)
    {
        return definitionById.TryGetValue(id, out var def) ? def : null;
    }
 
    public static int GetCount(string item)
    {
        return counts.TryGetValue(item, out int c) ? c : 0;
    }
 
    public static bool HasItem(string item, int amount = 1)
    {
        return GetCount(item) >= amount;
    }

    public static void AddCount(string item, int amount)
    {
        int newCount = GetCount(item) + amount;
        if (newCount <= 0) counts.Remove(item);
        else counts[item] = newCount;
    }

    public void OpenCloseInventory()
    {
        InvIsOpen = !InvIsOpen;
        InventoryMenu.SetActive(InvIsOpen);
    }
 
    //reset everything
    public void ResetAllInventory()
    {
        counts.Clear();
        if (slotById.Count > 0) RefreshDisplay();
    }

}

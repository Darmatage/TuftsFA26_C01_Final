using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestUI : MonoBehaviour
{
    [Header("UI Panels & Grids")]
    public GameObject chestPanel;
    public Transform chestSlotGrid;
    public Transform playerSlotGrid;
    public GameObject slotPrefab;

    [Header("References")]
    public PlayerInventory playerInventory;
    private StorageChest currentChest;

    public void OpenUI(StorageChest chest)
    {
        currentChest = chest;
        chestPanel.SetActive(true);
        RefreshUI();
    }

    public void CloseUI()
    {
        chestPanel.SetActive(false);
        currentChest = null;
    }

    public void RefreshUI()
    {
        ClearGrid(chestSlotGrid);
        ClearGrid(playerSlotGrid);

        if (currentChest == null || playerInventory == null) return;

        // Populate Chest Slots (Click to Take)
        foreach (InventorySlot slot in currentChest.storedItems)
        {
            CreateSlotButton(slot, chestSlotGrid, () => TransferToPlayer(slot));
        }

        // Populate Player Slots (Click to Store)
        foreach (InventorySlot slot in playerInventory.items)
        {
            CreateSlotButton(slot, playerSlotGrid, () => TransferToChest(slot));
        }
    }

    private void CreateSlotButton(InventorySlot slot, Transform grid, UnityEngine.Events.UnityAction onClickAction)
    {
        GameObject newSlot = Instantiate(slotPrefab, grid);
        Image iconImage = newSlot.transform.Find("Icon").GetComponent<Image>();
        TMP_Text countText = newSlot.transform.Find("Count").GetComponent<TMP_Text>();

        iconImage.sprite = slot.item.icon;
        countText.text = slot.amount > 1 ? slot.amount.ToString() : "";

        newSlot.GetComponent<Button>().onClick.AddListener(onClickAction);
    }

    private void TransferToPlayer(InventorySlot slot)
    {
        if (playerInventory.AddItem(slot.item, slot.amount))
        {
            currentChest.RemoveItem(slot);
            RefreshUI();
        }
    }

    private void TransferToChest(InventorySlot slot)
    {
        if (currentChest.AddItem(slot.item, slot.amount))
        {
            playerInventory.RemoveItem(slot);
            RefreshUI();
        }
    }

    private void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid) Destroy(child.gameObject);
    }
}

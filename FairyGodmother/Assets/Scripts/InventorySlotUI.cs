using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image iconImage;
    public TMP_Text countText;
    public TMP_Text nameText;   // optional
    public Vector2 ghostSize = new Vector2(64, 64);
 
    public string itemId { get; private set; }
 
    GameInventory inventory;
    int currentCount;
    RectTransform ghost;
    Canvas rootCanvas;


    //create a slot
    public void Setup(ItemDefinition def, GameInventory owner)
    {
        itemId = def.id;
        inventory = owner;
        if (iconImage != null) iconImage.sprite = def.icon;
        if (nameText != null) nameText.text = def.displayName;
    }

    //update
    public void Refresh(int count, bool showEmpty)
    {
        currentCount = count;
        gameObject.SetActive(count > 0 || showEmpty);
        if (iconImage != null) iconImage.enabled = count > 0;
        if (countText != null) countText.text = count > 0 ? count.ToString() : "";
    }

    //enables clicking to utilize as buff or ingredient
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && currentCount > 0)
            inventory.UseItem(itemId);
    }

    //CHECK IF WORKS, SHOULD BE DRAGGABLE, NEED TO ADD EXTRA STUFF
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || currentCount <= 0) return;
 
        var def = inventory.GetDefinition(itemId);
        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
 
        var go = new GameObject("DragGhost", typeof(RectTransform), typeof(CanvasGroup), typeof(Image));
        go.transform.SetParent(rootCanvas.transform, false);
        go.transform.SetAsLastSibling();
 
        var img = go.GetComponent<Image>();
        img.sprite = def != null ? def.icon : null;
        img.preserveAspect = true;
        img.raycastTarget = false;
        go.GetComponent<CanvasGroup>().blocksRaycasts = false; // so the pot can receive the drop
 
        ghost = (RectTransform)go.transform;
        ghost.sizeDelta = ghostSize;
        MoveGhost(eventData);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        if (ghost != null) MoveGhost(eventData);
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        DestroyGhost();
    }
 
    void OnDisable()
    {
        DestroyGhost();
    }
 
    //move the almost transparent copy withe cursor ideally
    void MoveGhost(PointerEventData eventData)
    {
        var canvasRect = (RectTransform)rootCanvas.transform;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRect, eventData.position, eventData.pressEventCamera, out Vector3 world))
        {
            ghost.position = world;
        }
    }
 
    void DestroyGhost()
    {
        if (ghost != null)
        {
            Destroy(ghost.gameObject);
            ghost = null;
        }
    }

}

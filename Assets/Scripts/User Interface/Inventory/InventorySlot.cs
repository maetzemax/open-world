using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

    public Image icon;
    public Button removeButton;
    public Text itemAmount;

    ItemObject itemObject;

    public void AddItem(ItemObject itemObject) {
        this.itemObject = itemObject;

        itemAmount.text = itemObject.item.itemAmount.ToString();
        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        itemObject = null;

        itemAmount.text = "";
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("OnDrop11");
        if (eventData.pointerDrag != null) {
            //RectTransform rectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //GetComponent<RectTransform>().anchoredPosition;
            InventorySlot movedItemSlot = eventData.pointerDrag.GetComponentInParent<InventorySlot>();
            ItemObject movedItem = eventData.pointerDrag.GetComponentInParent<InventorySlot>().itemObject;

            movedItemSlot.ClearSlot();

            if (itemObject != null) {
                movedItemSlot.AddItem(itemObject);
            }

            AddItem(movedItem);
        }
    }

    public void OnRemoveButton() {
        InventoryManager.instance.RemoveItem(itemObject);
    }

    public void UseItem() {
        if (itemObject != null) {
            itemObject.item.Use();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

    public Image icon;
    public Button removeButton;
    public Text itemAmount;
    public int slotID;

    public ItemObject itemObject;

    public bool isAssigned = false;

    public void AddItem(ItemObject itemObject) {
        this.itemObject = itemObject;
        isAssigned = true;

        itemAmount.text = itemObject.inventoryObject.itemAmount.ToString();
        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        itemObject = null;
        isAssigned = false;

        itemAmount.text = "";
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            InventorySlot movedItemSlot = eventData.pointerDrag.GetComponentInParent<InventorySlot>();
            ItemObject movedItem = eventData.pointerDrag.GetComponentInParent<InventorySlot>().itemObject;

            movedItemSlot.ClearSlot();

            if (itemObject != null) {
                movedItemSlot.AddItem(itemObject);
            }

            movedItem.inventoryObject.slotId = slotID;

            AddItem(movedItem);
            InventoryDataManager.instance.SaveData();
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

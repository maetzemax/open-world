using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

    public Image icon;
    public Button removeButton;
    public Text itemAmount;
    public int slotID;

    ItemObject itemObject;

    public void AddItem(ItemObject itemObject) {
        this.itemObject = itemObject;

        itemAmount.text = itemObject.item.itemAmount.ToString();
        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;

        InventoryDataManager inventoryDM = InventoryDataManager.instance;
        InventoryManager inventoryManager = InventoryManager.instance;
        List<InventoryObject> inventoryObjects = inventoryDM.inventoryObjectDB.inventoryObjects;
        List<ItemObject> inventoryObjectsManager = inventoryManager.itemList;

        InventoryObject currentObjectDM = inventoryObjects.Find(io => io.itemGUID == itemObject.inventoryObject.itemGUID);
        ItemObject currentObjectManager = inventoryObjectsManager.Find(io => io.inventoryObject.itemGUID == itemObject.inventoryObject.itemGUID);
        currentObjectDM.slotId = slotID;
        currentObjectManager.inventoryObject.slotId = slotID;

        inventoryDM.SaveData();
    }

    public void ClearSlot() {
        itemObject = null;

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

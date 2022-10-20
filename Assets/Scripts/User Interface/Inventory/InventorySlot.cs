using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler {

    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemAmount;
    public int slotID;

    public ItemObject itemObject;

    public bool isAssigned = false;

    public void AddItem(ItemObject itemObject) {

        var slots = Resources.FindObjectsOfTypeAll<HotbarSlot>();

        Array.Sort(slots, delegate (HotbarSlot slot1, HotbarSlot slot2) {
            return slot1.identifier.CompareTo(slot2.identifier);
        });

        foreach (var slot in slots) {
            if (slot.identifier == slotID) {
                slot.AddItem(itemObject);
            }
        }

        this.itemObject = itemObject;
        isAssigned = true;

        itemAmount.text = itemObject.inventoryObject.itemAmount.ToString();
        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {

        var slots = Resources.FindObjectsOfTypeAll<HotbarSlot>();

        Array.Sort(slots, delegate (HotbarSlot slot1, HotbarSlot slot2) {
            return slot1.identifier.CompareTo(slot2.identifier);
        });

        foreach (var slot in slots) {
            if (slot.identifier == slotID) {
                slot.ClearSlot();
            }
        }

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
        InventoryManager.instance.RemoveItem(itemObject, 1);
    }

    public void UseItem() {
        if (itemObject != null) {
            itemObject.item.Use();
        }
    }
}

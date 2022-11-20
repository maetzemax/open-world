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
    private InventoryUI inventoryUI;
    private InventoryDataManager inventoryDataManager;

    public GameObject endBoss;

    public bool isAssigned = false;

    private void Start() {
        inventoryUI = gameObject.GetComponentInParent<InventoryUI>();
        inventoryDataManager = InventoryDataManager.instance;
    }

    public void AddItem(ItemObject itemObject) {
        var slots = Resources.FindObjectsOfTypeAll<HotbarSlot>();

        Array.Sort(slots,
            delegate(HotbarSlot slot1, HotbarSlot slot2) { return slot1.identifier.CompareTo(slot2.identifier); });

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

        Array.Sort(slots,
            delegate(HotbarSlot slot1, HotbarSlot slot2) { return slot1.identifier.CompareTo(slot2.identifier); });

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
        if (itemObject.item != null) {
            itemObject.item.Use();

            var id = itemObject.item.id;
            var slots = inventoryDataManager.inventoryObjectDB.inventoryObjects;
            var shardSlots = new List<InventoryObject>();

            if (id == 14 || id == 15 || id == 16 || id == 17) {
                // CHECK IF ALL SHARDS ARE COLLECTED
                foreach (var slot in slots) {
                    var currentID = slot.itemID;

                    if (currentID == 14 || currentID == 15 || currentID == 16 || currentID == 17) {
                        shardSlots.Add(slot);
                    }
                }

                var earthShard = shardSlots.Find(p => p.itemID == 14);
                var waterShard = shardSlots.Find(p => p.itemID == 15);
                var fireShard = shardSlots.Find(p => p.itemID == 16);
                var airShard = shardSlots.Find(p => p.itemID == 17);

                if (earthShard != null && waterShard != null && fireShard != null && airShard != null) {
                    // TODO: Instantiate BOSS
                    print("Summon Boss");
                    Instantiate(endBoss,
                        GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.forward + Vector3.up,
                        Quaternion.identity);
                }
                else {
                    print("NOT ALL SHARDS COLLECTED");
                }
            }
        }
    }
}
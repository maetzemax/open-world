using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public Transform itemParent;
    public GameObject inventoryUI;

    InventoryManager inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start() {
        inventory = InventoryManager.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI() {
        foreach (var slot in slots) {
            foreach (var item in inventory.itemList) {
                if (item.inventoryObject.slotId == slot.slotID) {
                    slot.AddItem(item);
                } else if (slot.itemObject == null) {
                    slot.AddItem(item);
                }
            }

        }
    }
}

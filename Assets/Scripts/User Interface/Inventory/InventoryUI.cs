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
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.itemList.Count) {
                slots[inventory.itemList[i].inventoryObject.slotId - 1].AddItem(inventory.itemList[i]);
            } else {
                if (!slots[i].isAssigned)
                    slots[i].ClearSlot();
            }
        }
    }
}

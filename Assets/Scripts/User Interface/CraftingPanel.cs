using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class CraftingPanel : MonoBehaviour {
    public TextMeshProUGUI title;
    public Image icon;

    public TextMeshProUGUI ingredient1;
    public TextMeshProUGUI ingredient2;

    InventoryManager inventory;
    ItemDatabase itemDatabase;

    Item craftingItem;
    CraftingRecipe craftingRecipe;

    private void Awake() {
        itemDatabase = ItemDatabase.instance;
        inventory = InventoryManager.instance;
    }

    public void AddRecipe(CraftingRecipe craftingRecipe) {
        this.craftingRecipe = craftingRecipe;

        craftingItem = itemDatabase.itemList.Find(i => i.id == craftingRecipe.resultItemID);

        icon.sprite = craftingItem.icon;
        icon.enabled = true;

        title.text = craftingItem.name;

        for (int i = 0; i < craftingRecipe.ingredients.Count; i++) {
            if (i == 0)
                ingredient1.text = craftingRecipe.ingredients[i].amount + "x " + itemDatabase.itemList
                    .Find(item => item.id == craftingRecipe.ingredients[i].itemID).name;
            if (i == 1)
                ingredient2.text = craftingRecipe.ingredients[i].amount + "x " + itemDatabase.itemList
                    .Find(item => item.id == craftingRecipe.ingredients[i].itemID).name;
        }
    }

    public void CraftRecipe() {
        for (var i = 0; i < craftingRecipe.ingredients.Count; i++) {
            switch (i) {
                case 0:
                    CalculateCrafting(i);
                    break;
                case 1:
                    CalculateCrafting(i);
                    break;
            }
        }
    }

    public void CalculateCrafting(int index) {
        int ingredientAmount = craftingRecipe.ingredients[index].amount;

        var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();

        Array.Sort(slots,
            delegate(InventorySlot slot1, InventorySlot slot2) { return slot1.slotID.CompareTo(slot2.slotID); });

        List<InventorySlot> filteredSlots = new();

        foreach (var slot in slots) {
            if (slot.itemObject != null)
                if (slot.itemObject.inventoryObject.itemID == craftingRecipe.ingredients[index].itemID) {
                    filteredSlots.Add(slot);
                }
        }

        var itemAmount = filteredSlots.Sum(slot => slot.itemObject.inventoryObject.itemAmount);

        if (itemAmount < ingredientAmount)
            return;

        foreach (var filteredSlot in filteredSlots) {
            if (ingredientAmount < filteredSlot.itemObject.inventoryObject.itemAmount) {
                inventory.RemoveItem(filteredSlot.itemObject, ingredientAmount);
                break;
            }
            else if (ingredientAmount >= filteredSlot.itemObject.inventoryObject.itemAmount) {
                ingredientAmount -= filteredSlot.itemObject.inventoryObject.itemAmount;
                inventory.RemoveItem(filteredSlot.itemObject, filteredSlot.itemObject.inventoryObject.itemAmount);
            }

            if (ingredientAmount <= 0) {
                break;
            }
        }

        if (index == craftingRecipe.ingredients.Count - 1) {
            var allSlots = Resources.FindObjectsOfTypeAll<InventorySlot>();

            Array.Sort(allSlots,
                delegate(InventorySlot slot1, InventorySlot slot2) { return slot1.slotID.CompareTo(slot2.slotID); });

            int slotID = 1;

            foreach (var slot in allSlots) {
                if (!slot.isAssigned && slot.slotID != 0) {
                    slotID = slot.slotID;
                    break;
                }
            }

            inventory.AddItem(new ItemObject(itemDatabase.itemList.Find(item => item.id == craftingRecipe.resultItemID),
                new InventoryObject(craftingRecipe.resultItemID, craftingRecipe.resultAmount, slotID)));

            WorldDataManager.instance.SaveData();
        }
    }
}
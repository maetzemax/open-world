using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    #region Singleton

    public static InventoryManager instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance InventoryManager found");
            return;
        }

        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public Text pickUpText;
    public int space = 20;
    public List<ItemObject> itemList;

    [SerializeField] GameObject inventory;

    private InventoryDataManager inventoryDataManager;

    private bool isInventoryLoaded = false;

    private void Start() {
        pickUpText.enabled = false;
        inventoryDataManager = InventoryDataManager.instance;
    }

    public void Update() {
        List<InventoryObject> inventoryObjects = InventoryDataManager.instance.inventoryObjectDB.inventoryObjects;

        if (itemList.Count == 0 && !isInventoryLoaded) {
            LoadInventory(inventoryObjects);
        }
    }

    public void AddItem(ItemObject itemObject) {

        if (itemList.Count >= space) {
            return;
        }

        var currentItems = itemList.FindAll(item => item.inventoryObject.itemID == itemObject.inventoryObject.itemID && item.inventoryObject.itemAmount < item.item.stackSize);

        if (currentItems.Count > 0) {
            var item = ItemDatabase.instance.itemList.Find(it => it.id == currentItems[0].inventoryObject.itemID);
            if (currentItems[0].inventoryObject.itemAmount + itemObject.inventoryObject.itemAmount <= item.stackSize) {
                currentItems[0].inventoryObject.itemAmount += itemObject.inventoryObject.itemAmount;
            } else {
                var extraAmount = currentItems[0].inventoryObject.itemAmount + itemObject.inventoryObject.itemAmount - item.stackSize;
                currentItems[0].inventoryObject.itemAmount = item.stackSize;
                ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, extraAmount, itemObject.inventoryObject.slotId));
                itemList.Add(newItem);
                inventoryDataManager.AddInventoryObject(newItem.inventoryObject);
            }
            
        } else {
            ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, itemObject.inventoryObject.itemAmount, itemObject.inventoryObject.slotId));
            itemList.Add(newItem);
            inventoryDataManager.AddInventoryObject(newItem.inventoryObject);
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        inventoryDataManager.SaveData();
    }

    public void RemoveItem(ItemObject itemObject, int amount) {

        itemObject.inventoryObject.itemAmount -= amount;

        if (itemObject.inventoryObject.itemAmount <= 0) {
            itemList.Remove(itemObject);
            inventoryDataManager.RemoveInventoryObject(itemObject.inventoryObject);

            var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();
            foreach (var slot in slots) {
                if (slot.slotID == itemObject.inventoryObject.slotId) {
                    slot.ClearSlot();
                }
            }
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        inventoryDataManager.SaveData();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {

        foreach (var itemObject in inventoryObjects) {

            var newItem = new ItemObject(ItemDatabase.instance.itemList.Find(p => p.id == itemObject.itemID), itemObject);
            itemList.Add(newItem);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }

        isInventoryLoaded = true;
    }
}

[System.Serializable]
public class ItemObject {
    public Item item;
    public InventoryObject inventoryObject;

    public ItemObject(Item item, InventoryObject inventoryObject) {
        this.item = item;
        this.inventoryObject = inventoryObject;
    }
}

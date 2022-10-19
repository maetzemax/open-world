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

    private bool isInventoryLoaded = false;

    private void Start() {
        pickUpText.enabled = false;
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

        ItemObject currentItem = itemList.Find(item => item.inventoryObject.itemID == itemObject.inventoryObject.itemID);

        if (currentItem != null) {
            if (currentItem.inventoryObject.itemAmount == currentItem.item.stackSize) {
                ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1, itemObject.inventoryObject.slotId));
                itemList.Add(newItem);
                InventoryDataManager.instance.AddInventoryObject(newItem.inventoryObject);
            } else {
                currentItem.inventoryObject.itemAmount++;
            }
        } else {
            ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1, itemObject.inventoryObject.slotId));
            itemList.Add(newItem);
            InventoryDataManager.instance.AddInventoryObject(newItem.inventoryObject);
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    public void RemoveItem(ItemObject itemObject) {

        ItemObject currentItem = itemList.Find(item => item.inventoryObject.itemGUID == itemObject.inventoryObject.itemGUID);

        if (currentItem.inventoryObject.itemAmount == 0) {
            itemList.Remove(itemObject);
            InventoryDataManager.instance.RemoveInventoryObject(itemObject.inventoryObject);
        } else if (currentItem.inventoryObject.itemAmount > 0) {
            currentItem.inventoryObject.itemAmount--;
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {

        print("Inventory loaded");

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

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

    private void Start() {
        pickUpText.enabled = false;
    }

    public void Update() {
        List<InventoryObject> inventoryObjects = InventoryDataManager.instance.inventoryObjectDB.inventoryObjects;

        if (itemList.Count == 0) {
            LoadInventory(inventoryObjects);
        }
    }

    public void AddItem(ItemObject itemObject) {

        if (itemList.Count >= space) {
            return;
        }

        ItemObject currentItem = itemList.Find(item => item.inventoryObject.itemGUID == itemObject.inventoryObject.itemGUID);

        if (currentItem != null) {
            if (currentItem.inventoryObject.itemAmount == currentItem.item.stackSize) {
                ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1));
                itemList.Add(newItem);
                InventoryDataManager.instance.AddInventoryObject(newItem.inventoryObject);
            } else {
                currentItem.inventoryObject.itemAmount++;
            }
        } else {
            ItemObject newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1));
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
        foreach (var itemObject in inventoryObjects) {

            var newItem = new ItemObject(ItemDatabase.instance.itemList.Find(p => p.id == itemObject.itemID), itemObject);
            itemList.Add(newItem);            

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }

        foreach (var item in itemList) {
            print("slot id = " + item.inventoryObject.slotId);
        }

        CheckInventoryPosition();
    }

    void CheckInventoryPosition() {
        InventorySlot[] inventorySlots = inventory.GetComponentsInChildren<InventorySlot>();

        foreach (var inventorySlot in inventorySlots) {
            inventorySlot.ClearSlot();

            foreach (var itemObjc in itemList) {
                print(itemObjc.inventoryObject.slotId );
                if (inventorySlot.slotID == itemObjc.inventoryObject.slotId) {                
                    inventorySlot.AddItem(itemObjc);
                }
            }
        }

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

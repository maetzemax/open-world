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

        itemList.Add(itemObject);
        InventoryDataManager.instance.AddInventoryObject(itemObject.inventoryObject);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    public void RemoveItem(ItemObject itemObject) {


        itemList.Remove(itemObject);
        InventoryDataManager.instance.RemoveInventoryObject(itemObject.inventoryObject);


        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {
        foreach (var itemObject in inventoryObjects) {

            itemList.Add(new ItemObject(ItemDatabase.instance.itemList.Find(p => p.id == itemObject.itemID), new InventoryObject(itemObject.itemID, itemObject.itemGUID)));

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
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

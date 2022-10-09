using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    #region Singleton

    public static InventoryManager instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance Inventory found");
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

    private bool isInventoryLoaded = false;

    private void Start() {
        pickUpText.enabled = false;
    }

    public void Update() {
        List<InventoryObject> inventoryObjects = InventoryDataManager.instance.inventoryObjectDB.inventoryObjects;

        if (inventoryObjects.Count != 0 && !isInventoryLoaded) {
            LoadInventory(inventoryObjects);
        }
    }

    public void AddItem(ItemObject item) {

        if (itemList.Count >= space) {
            return;
        }

        itemList.Add(item);

        for (int i = 0; i < itemList.Count; i++) { 
            if (itemList[i] == item) {
                InventoryDataManager.instance.AddInventoryObject(new InventoryObject(item.item.id));
            }
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    public void RemoveItem(ItemObject item) {


        itemList.Remove(item);
        InventoryDataManager.instance.RemoveInventoryObject(item.inventoryObject);


        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {
        foreach (var item in inventoryObjects) {

            itemList.Add(new ItemObject(ItemDatabase.instance.itemList.Find(p => p.id == item.itemID), new InventoryObject(item.itemID, item.itemGUID)));

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    #region Singleton

    public static InventoryManager instance;

    public bool isInventoryLoaded = false;

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
    public List<Item> itemList;

    private void Start() {
        pickUpText.enabled = false;
    }

    public void Update() {
        List<InventoryObject> inventoryObjects = InventoryDataManager.instance.inventoryObjectDB.inventoryObjects;

        if (inventoryObjects.Count != 0 && !isInventoryLoaded) {
            LoadInventory(inventoryObjects);
        }
    }

    public void AddItem(Item item) {

        if (itemList.Count >= space) {
            return;
        }

        itemList.Add(item);
        InventoryDataManager.instance.AddInventoryObject(new InventoryObject(item.id));
        InventoryDataManager.instance.SaveData();

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(Item item) {
        itemList.Remove(item);
        InventoryDataManager.instance.RemoveInventoryObject(new InventoryObject(item.id));
        InventoryDataManager.instance.SaveData();

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {
        foreach (var item in inventoryObjects) {

            itemList.Add(ItemDatabase.instance.itemList.Find(p => p.id == item.itemID));

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }

        isInventoryLoaded = true;
    }

}

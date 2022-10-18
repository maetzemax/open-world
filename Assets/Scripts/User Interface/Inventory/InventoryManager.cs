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

    private bool isSlotIDLoaded = false;

    private void Start() {
        pickUpText.enabled = false;
    }

    public void Update() {
        List<InventoryObject> inventoryObjects = InventoryDataManager.instance.inventoryObjectDB.inventoryObjects;

        if (itemList.Count == 0) {
            LoadInventory(inventoryObjects);
        } else if (inventory.activeSelf && !isSlotIDLoaded) {
            CheckInventoryPosition();
        }
    }

    public void AddItem(ItemObject itemObject) {

        if (itemList.Count >= space) {
            return;
        }

        Item copyItem = Instantiate(itemObject.item);

        if (itemObject.item.itemAmount == 1) {
            itemList.Add(itemObject);
            InventoryDataManager.instance.AddInventoryObject(itemObject.inventoryObject);
        }

        foreach (var currentItem in itemList) {
            if (currentItem.item.name == copyItem.name) {
                itemObject.item.itemAmount++;
                break;
            }
        }

        print(itemObject.item.itemAmount);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    public void RemoveItem(ItemObject itemObject) {

        Item copyItem = Instantiate(itemObject.item);

        if (itemObject.item.itemAmount == 1) {
            itemList.Remove(itemObject);

            InventoryDataManager.instance.RemoveInventoryObject(itemObject.inventoryObject);
        }

        foreach (var currentItem in itemList) {
            if (currentItem.item.name == copyItem.name) {
                itemObject.item.itemAmount--;
                break;
            }
        }

        print(itemObject.item.itemAmount);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        InventoryDataManager.instance.SaveData();
    }

    void LoadInventory(List<InventoryObject> inventoryObjects) {
        foreach (var itemObject in inventoryObjects) {

            itemList.Add(
                new ItemObject(ItemDatabase.instance.itemList.Find(p => p.id == itemObject.itemID),
                new InventoryObject(itemObject.itemID, itemObject.itemGUID, itemObject.slotId)
                ));

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    void CheckInventoryPosition() {
        InventorySlot[] inventorySlots = inventory.GetComponentsInChildren<InventorySlot>();

        foreach (var inventorySlot in inventorySlots) {
            foreach (var itemObjc in itemList) {
                if (itemObjc.inventoryObject.slotId == inventorySlot.slotID) {
                    ItemObject inventoryObjc = itemObjc;
                    int index = itemList.IndexOf(itemObjc);
                    print("Object Found");

                    inventorySlots[index].ClearSlot();
                    inventorySlot.AddItem(inventoryObjc);
                }
            }
        }

        isSlotIDLoaded = true;

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

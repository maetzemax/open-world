using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

public class InventoryDataManager : MonoBehaviour {

    #region Singleton

    public static InventoryDataManager instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance InventoryDataManager found");
            return;
        }

        instance = this;
    }

    #endregion

    void Start() {
        LoadData();
    }

    public InventoryDatabase inventoryObjectDB;

    public void AddInventoryObject(InventoryObject inventoryObject) {
        inventoryObjectDB.inventoryObjects.Add(inventoryObject);
    }

    public void RemoveInventoryObject(InventoryObject inventoryObject) {
        for (int i = 0; i < inventoryObjectDB.inventoryObjects.Count; i++) {
            if (inventoryObjectDB.inventoryObjects[i].itemGUID == inventoryObject.itemGUID) {
                inventoryObjectDB.inventoryObjects.RemoveAt(i);
            }
        }
    }

    public void SaveData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(InventoryDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Inventory_Data.xml", FileMode.Create);
        xmlSerializer.Serialize(stream, inventoryObjectDB);
        stream.Close();
    }

    void LoadData() {
        if (!File.Exists(Application.dataPath + "/Inventory_Data.xml")) { return; }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(InventoryDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Inventory_Data.xml", FileMode.Open);
        inventoryObjectDB = xmlSerializer.Deserialize(stream) as InventoryDatabase;
        stream.Close();
    }
}

[System.Serializable]
public class InventoryDatabase {

    public List<InventoryObject> inventoryObjects = new List<InventoryObject>();
}

[System.Serializable]
public class InventoryObject {

    public int itemID;
    public string itemGUID;
    public int slotId;
    public int itemAmount;

    public InventoryObject(int itemID, int itemAmount) {
        this.itemID = itemID;
        this.itemAmount = itemAmount;
        itemGUID = System.Guid.NewGuid().ToString();
    }

    public InventoryObject(int itemID, string itemGUID, int itemAmount) {
        this.itemID = itemID;
        this.itemGUID = itemGUID;
        this.itemAmount = itemAmount;
    }

    public InventoryObject(int itemID, string itemGUID, int slotId, int itemAmount) {
        this.itemID = itemID;
        this.itemGUID = itemGUID;
        this.slotId = slotId;
        this.itemAmount = itemAmount;
    }

    public InventoryObject() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Linq;
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
        inventoryObjectDB.inventoryObjects.Remove(inventoryObject);
    }

    public void SaveData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(InventoryDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Inventory_Data.xml", FileMode.Create);
        xmlSerializer.Serialize(stream, inventoryObjectDB);
        stream.Close();
    }

    void LoadData() {
        if (!File.Exists(Application.dataPath + "/Inventory_Data.xml")) { return; }
        print(Application.dataPath);
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

    public InventoryObject(int itemID) {
        this.itemID = itemID;
    }

    public InventoryObject() {
    }
}

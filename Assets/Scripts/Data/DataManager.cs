using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Linq;
using System.Xml.Serialization;
using System.IO;


public class DataManager : MonoBehaviour {

    #region Singleton

    public static DataManager instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance Inventory found");
            return;
        }

        instance = this;
    }

    #endregion

    void Start() {
        LoadData();
    }

    public WorldObjectDatabase worldObjectDB;

    public void AddWorldObject(WorldObject worldObject) {
        worldObjectDB.worldObjects.Add(worldObject);
    }

    public void RemoveWorldObject(WorldObject worldObject) {
        worldObjectDB.worldObjects.Remove(worldObject);
    }

    public void SaveData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorldObjectDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Stream Files/Game_Data.xml", FileMode.Create);
        xmlSerializer.Serialize(stream, worldObjectDB);
        stream.Close();
    }

    void LoadData() {
        if (!File.Exists(Application.dataPath + "/Stream Files/Game_Data.xml")) { return; }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorldObjectDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Stream Files/Game_Data.xml", FileMode.Open);
        worldObjectDB = xmlSerializer.Deserialize(stream) as WorldObjectDatabase;
        stream.Close();

        StartCoroutine(WaitForWorldRenderer());
    }

    IEnumerator WaitForWorldRenderer() {
        yield return new WaitForSeconds(1f);

        foreach (var item in worldObjectDB.worldObjects) {
            PrefabItem prefabItem = PrefabDatabase.instance.prefabItems.Where(p => p.prefabID == item.prefabID).First();

            RaycastHit hit;
            Ray ray = new(item.worldPosition + new Vector3(0, 100, 0), Vector3.down);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                GameObject go = Instantiate(prefabItem.prefabGameobject, item.worldPosition, item.orientation, hit.transform);
                go.name = prefabItem.prefabID;
            }
        }
    }
}

[System.Serializable]
public class WorldObjectDatabase {

    public List<WorldObject> worldObjects = new List<WorldObject>();

}

[System.Serializable]
public class WorldObject {

    public string prefabID;
    public string terrainID;
    public Vector3 localPosition;
    public Vector3 worldPosition;
    public Quaternion orientation;

    public WorldObject(string prefabID, string terrainID, Vector3 worldPosition, Vector3 localPosition, Quaternion orientation) {
        this.prefabID = prefabID;
        this.terrainID = terrainID;
        this.worldPosition = worldPosition;
        this.localPosition = localPosition;
        this.orientation = orientation;
    }

    public WorldObject() {
    }
}

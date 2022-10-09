using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

public class WorldDataManager : MonoBehaviour {

    #region Singleton

    public static WorldDataManager instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance WorldDataManager found");
            return;
        }

        instance = this;
    }

    #endregion

    public WorldObjectDatabase worldObjectDB;

    void Start() {
        LoadData();
    }

    public void AddWorldObject(WorldObject worldObject) {
        worldObjectDB.worldObjects.Add(worldObject);
    }

    public void RemoveWorldObject(WorldObject worldObject) {
        worldObjectDB.worldObjects.Remove(worldObject);
    }

    public void SaveData() {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorldObjectDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Game_Data.xml", FileMode.Create);
        xmlSerializer.Serialize(stream, worldObjectDB);
        stream.Close();
    }

    void LoadData() {
        if (!File.Exists(Application.dataPath + "/Game_Data.xml")) { return; }
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(WorldObjectDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Game_Data.xml", FileMode.Open);
        worldObjectDB = xmlSerializer.Deserialize(stream) as WorldObjectDatabase;
        stream.Close();
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
    public Vector3 worldPosition;
    public Quaternion orientation;
    public bool isDestroyed;
    public int health;

    public WorldObject(string prefabID, string terrainID, Vector3 worldPosition, Quaternion orientation, bool isDestroyed, int health) {
        this.prefabID = prefabID;
        this.terrainID = terrainID;
        this.worldPosition = worldPosition;
        this.orientation = orientation;
        this.isDestroyed = isDestroyed;
        this.health = health;
    }

    public WorldObject() { }
}

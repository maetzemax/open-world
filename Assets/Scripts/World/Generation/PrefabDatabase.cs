using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDatabase : MonoBehaviour {
    #region Singleton

    public static PrefabDatabase instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of PrefabDatabase found");
            return;
        }

        instance = this;
    }

    #endregion

    public List<PrefabItem> prefabItems = new List<PrefabItem>();
}

[System.Serializable]
public class PrefabItem {
    public GameObject prefabGameobject;
    public Vector3 prefabSize;
    public string prefabID;
}
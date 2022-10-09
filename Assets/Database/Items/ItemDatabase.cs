using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    #region Singleton

    public static ItemDatabase instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance ItemDatabase found");
            return;
        }

        instance = this;
    }

    #endregion

    public List<Item> itemList;
}

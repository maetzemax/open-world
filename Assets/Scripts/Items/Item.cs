using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "open-world/Inventory/Item")]
[System.Serializable]
public class Item : ScriptableObject {
    public new string name;
    public int id;
    public Sprite icon;

    public virtual void Use() {
        // USE ITEM
        // SOMETHING SHOULD HAPPEND
        Debug.Log("Use item");
    }
}

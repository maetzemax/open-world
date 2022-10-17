using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "open-world/Inventory/Item")]
public class Item : ScriptableObject {
    public new string name;
    public int id;
    public Sprite icon;

    public int itemAmount = 1;
    public int stackSize = 1;

    public virtual void Use() {
        // USE ITEM
        // SOMETHING SHOULD HAPPEND
        Debug.Log("Use item");
    }
}

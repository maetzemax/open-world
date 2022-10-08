using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {
    public Item drop;
    public int health = 5;

    public virtual void Harvest() {


        // TODO: Drop Item on the Ground if Inventory is full
        InventoryManager.instance.AddItem(drop);

        health--;

        if (health == 0) {
            Destroy(gameObject);
        }
    }
}

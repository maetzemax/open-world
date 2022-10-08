using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {

    public Item drop;
    public int health = 5;

    InventoryManager inventory;

    private void Start() {
        inventory = InventoryManager.instance;
    }

    public virtual void Harvest() {


        // TODO: Drop Item on the Ground if Inventory is full
        inventory.AddItem(drop);

        health--;

        if (health == 0) {
            Destroy(gameObject);
        }
    }
}

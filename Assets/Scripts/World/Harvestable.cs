using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {
    public Item drop;
    public int health = 5;

    public virtual void Harvest() {

        InventoryManager.instance.itemList.Add(drop);

        health--;

        if (health == 0) {
            Destroy(gameObject);
        }
    }
}

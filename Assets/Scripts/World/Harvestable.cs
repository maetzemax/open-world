using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {

    public ItemObject drop;
    public int health = 5;

    public string prefabID;

    InventoryManager inventory;

    private void Start() {
        inventory = InventoryManager.instance;

        WorldObject worldObject = WorldDataManager.instance.worldObjectDB.worldObjects.Find(p => p.worldPosition == transform.position);

        if(worldObject != null)
            health = worldObject.health;
    }

    public virtual void Harvest() {


        // TODO: Drop Item on the Ground if Inventory is full
        

        health--;

        GameObject terrainTile = GetComponentInParent<GenerateMesh>().gameObject;

        List<WorldObject> worldObjects = WorldDataManager.instance.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainTile.name);

        if (filteredObjects.Count == 0) {

            GameObject tile = gameObject.GetComponentInParent<PlaceObjects>().gameObject;

            foreach (Transform child in tile.transform)
                saveGameObject(child.gameObject);

            WorldDataManager.instance.SaveData();

        } else {

            print("Updated Database");

            // Remove current
            WorldObject worldObject = WorldDataManager.instance.worldObjectDB.worldObjects.Find(p => p.worldPosition == transform.position);
            worldObject.health = health;

            // Save
            WorldDataManager.instance.SaveData();
        }

        if (health == 0) {

            // Remove current destroyed one
            WorldObject worldObject = WorldDataManager.instance.worldObjectDB.worldObjects.Find(p => p.worldPosition == transform.position);
            worldObject.isDestroyed = true;

            // Save
            WorldDataManager.instance.SaveData();

            Destroy(gameObject);
        }

        inventory.AddItem(drop);
    }

    private void saveGameObject(GameObject gameObject) {
        Harvestable harvestable = gameObject.GetComponent<Harvestable>();
        GameObject terrainTile = gameObject.GetComponentInParent<GenerateMesh>().gameObject;
        WorldDataManager.instance.AddWorldObject(new WorldObject(harvestable.prefabID, terrainTile.name, gameObject.transform.position, gameObject.transform.rotation, false, harvestable.health));
    }
}

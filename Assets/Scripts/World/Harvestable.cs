using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour {

    public ItemObject drop;
    public int health = 5;

    public Category destrutable;

    InventoryManager inventory;

    private void Start() {
        inventory = InventoryManager.instance;

        WorldObject worldObject = WorldDataManager.instance.worldObjectDB.worldObjects.Find(p => p.worldPosition == transform.position);

        if(worldObject != null)
            health = worldObject.health;
    }

    public void Harvest() {


        // TODO: Drop Item on the Ground if Inventory is full

        health--;

        GameObject terrainTile = GetComponentInParent<GenerateMesh>().gameObject;

        List<WorldObject> worldObjects = WorldDataManager.instance.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainTile.name);

        if (filteredObjects.Count == 0) {

            GameObject tile = gameObject.GetComponentInParent<PlaceObjects>().gameObject;

            foreach (Transform child in tile.transform)
                SaveGameObject(child.gameObject);

            var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();

            Array.Sort(slots, delegate (InventorySlot slot1, InventorySlot slot2) {
                return slot1.slotID.CompareTo(slot2.slotID);
            });

            int slotID = 1;

            foreach (var slot in slots) {
                if (!slot.isAssigned && slot.slotID != 0) {
                    slotID = slot.slotID;
                    break;
                }
            }

            inventory.AddItem(new ItemObject(drop.item, new InventoryObject(drop.item.id, 1, slotID)));

            WorldDataManager.instance.SaveData();

        } else {

            // Remove current
            PrefabIdentifier prefabIdentifier = gameObject.GetComponentInParent<PrefabIdentifier>();

            if (prefabIdentifier == null) {
                prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
            }
            
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == prefabIdentifier.gameObject.transform.position);
            worldObject.health = health;

            var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();

            Array.Sort(slots, delegate (InventorySlot slot1, InventorySlot slot2) {
                return slot1.slotID.CompareTo(slot2.slotID);
            });

            int slotID = 1;

            foreach (var slot in slots) {
                if (!slot.isAssigned && slot.slotID != 0) {
                    slotID = slot.slotID;
                    break;
                }
            }

            inventory.AddItem(new ItemObject(drop.item, new InventoryObject(drop.item.id, 1, slotID)));

            // Save
            WorldDataManager.instance.SaveData();
        }

        if (health == 0) {

            // Remove current destroyed one
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == transform.position);
            WorldDataManager.instance.RemoveWorldObject(worldObject);

            // Save
            WorldDataManager.instance.SaveData();

            Destroy(gameObject);
        }
    }

    void SaveGameObject(GameObject harvestGameObject) {
        PrefabIdentifier prefabIdentifier = harvestGameObject.GetComponentInParent<PrefabIdentifier>();

        if (prefabIdentifier == null) {
            prefabIdentifier = harvestGameObject.GetComponent<PrefabIdentifier>();
        }

        GameObject terrainTile = harvestGameObject.GetComponentInParent<GenerateMesh>().gameObject;

        WorldDataManager.instance.AddWorldObject(new WorldObject(prefabIdentifier.prefabIdentifier, terrainTile.name, prefabIdentifier.transform.position, prefabIdentifier.transform.rotation, health));
    }
}

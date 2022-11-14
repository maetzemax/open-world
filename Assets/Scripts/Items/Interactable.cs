using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public ItemObject itemObject;
    InventoryManager inventory;
    WorldDataManager worldDataManager;

    private int health = 1;

    private void Awake() {
        inventory = InventoryManager.instance;
        worldDataManager = WorldDataManager.instance;
    }

    public virtual void Interact() {

        inventory.pickUpText.text = "";
        inventory.pickUpText.enabled = false;

        GameObject terrainTile = GetComponentInParent<GenerateMesh>().gameObject;

        List<WorldObject> worldObjects = worldDataManager.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainTile.name);

        health--;
        Destroy(gameObject);

        if (filteredObjects.Count == 0) {

            GameObject tile = gameObject.GetComponentInParent<PlaceObjects>().gameObject;
            
            foreach (Transform child in tile.transform) {

                Harvestable harvestable = child.gameObject.GetComponent<Harvestable>();
                if (harvestable != null) {
                    SaveGameObject(child.gameObject, tile.name, harvestable.health);
                } else if (harvestable == null) {
                    SaveGameObject(child.gameObject, tile.name, health);
                }
            }

            worldDataManager.SaveData();

        } else {

            // Remove current
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == transform.position);
            worldDataManager.RemoveWorldObject(worldObject);
            Destroy(gameObject);

            // Save
            worldDataManager.SaveData();
        }

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

        var newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1, slotID));

        inventory.AddItem(newItem);
    }

    private void SaveGameObject(GameObject gameObject, string tileName, int health) {
        PrefabIdentifier prefabIdentifier = gameObject.GetComponentInParent<PrefabIdentifier>();
        WorldObject worldObject = new(prefabIdentifier.prefabIdentifier, tileName, gameObject.transform.position, gameObject.transform.rotation, health);
        worldDataManager.AddWorldObject(worldObject);
    }
}

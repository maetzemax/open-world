using System.Collections;
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
                    saveGameObject(child.gameObject, tile.name, harvestable.health);
                } else if (harvestable == null) {
                    saveGameObject(child.gameObject, tile.name, health);
                }
            }

            worldDataManager.SaveData();

        } else {

            print("Updated Database");

            // Remove current
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == transform.position);
            worldDataManager.RemoveWorldObject(worldObject);
            Destroy(gameObject);

            // Save
            worldDataManager.SaveData();
        }

        var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();
        int slotID = 1;

        foreach (var slot in slots) {
            //if (slot.itemObject.item == null) {
            //    slotID = slot.slotID;
            //    break;
            //}
        }

        var newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, 1, slotID));

        inventory.AddItem(newItem);

        print("Item added to Inventory to slot: " + newItem.inventoryObject.slotId);
    }

    private void saveGameObject(GameObject gameObject, string tileName, int health) {
        PrefabIdentifier prefabIdentifier = gameObject.GetComponentInParent<PrefabIdentifier>();
        WorldObject worldObject = new(prefabIdentifier.prefabIdentifier, tileName, gameObject.transform.position, gameObject.transform.rotation, health);
        worldDataManager.AddWorldObject(worldObject);
    }
}

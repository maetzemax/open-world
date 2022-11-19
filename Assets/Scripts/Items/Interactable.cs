using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour {
    public int health = 1;
    public ItemObject firstItemDrop;
    [Range(0, 1)] [SerializeField] private float dropChanceFirstItem;
    [SerializeField] private int amountFirstItem;
    public ItemObject secondItemDrop;
    [Range(0, 1)] [SerializeField] private float dropChanceSecondItem;
    [SerializeField] private int amountSecondItem;
    private InventoryManager inventory;
    private WorldDataManager worldDataManager;

    private void Awake() {
        inventory = InventoryManager.instance;
        worldDataManager = WorldDataManager.instance;
    }

    public void Interact() {
        health = 0;

        inventory.pickUpText.text = "";
        inventory.pickUpText.enabled = false;

        var terrainTile = GetComponentInParent<GenerateMesh>().gameObject;

        List<WorldObject> worldObjects = worldDataManager.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainTile.name);

        if (filteredObjects.Count == 0) {
            var tile = gameObject.GetComponentInParent<PlaceObjects>().gameObject;

            foreach (Transform child in tile.transform) {
                var prefabIdentifier = child.gameObject.GetComponent<PrefabIdentifier>();

                if (prefabIdentifier != null) {
                    Harvestable harvestable = prefabIdentifier.gameObject.GetComponentInChildren<Harvestable>();
                    Interactable interactable = prefabIdentifier.gameObject.GetComponentInChildren<Interactable>();
                    
                    if (harvestable != null) {
                        SaveGameObject(prefabIdentifier.gameObject, tile.name, harvestable.health, prefabIdentifier.transform.position, false);
                    }
                    else if (interactable != null && prefabIdentifier.prefabIdentifier != "altar") {
                        SaveGameObject(prefabIdentifier.gameObject, tile.name, interactable.health, prefabIdentifier.transform.position, false);
                    } else if (prefabIdentifier.prefabIdentifier == "altar" && interactable.health <= 0) {
                        SaveGameObject(prefabIdentifier.gameObject, tile.name, 1, prefabIdentifier.transform.position, true);
                    }
                    else {
                        SaveGameObject(prefabIdentifier.gameObject, tile.name, 1, prefabIdentifier.transform.position, false);
                    }
                }
            }

            worldDataManager.SaveData();
        }
        else {
            // Remove current
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == gameObject.GetComponentInParent<PrefabIdentifier>().gameObject.transform.position);
            worldDataManager.RemoveWorldObject(worldObject);
            Destroy(gameObject);

            // Save
            worldDataManager.SaveData();
        }
        
        Destroy(gameObject);
        
        if (firstItemDrop != null) {
            CheckForSlot(firstItemDrop, dropChanceFirstItem, amountFirstItem);
        }

        if (secondItemDrop != null) {
            CheckForSlot(secondItemDrop, dropChanceSecondItem, amountSecondItem);
        }
    }

    private void CheckForSlot(ItemObject itemObject, float dropChance, int dropAmount) {
        var calculatedDropChance = 100 * dropChance;
        var randomNumber = Random.Range(0, 100);

        if (randomNumber <= calculatedDropChance) {
            var slots = Resources.FindObjectsOfTypeAll<InventorySlot>();

            Array.Sort(slots,
                delegate(InventorySlot slot1, InventorySlot slot2) { return slot1.slotID.CompareTo(slot2.slotID); });

            int slotID = 1;

            foreach (var slot in slots) {
                if (!slot.isAssigned && slot.slotID != 0) {
                    slotID = slot.slotID;
                    break;
                }
            }

            var newItem = new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id, dropAmount, slotID));
            inventory.AddItem(newItem);
        }
    }

    private void SaveGameObject(GameObject gameObject, string tileName, int health, Vector3 position, bool isCollected) {
        PrefabIdentifier prefabIdentifier = gameObject.GetComponentInParent<PrefabIdentifier>();
        WorldObject worldObject = new(prefabIdentifier.prefabIdentifier, tileName, position,
            gameObject.transform.rotation, health, isCollected);
        worldDataManager.AddWorldObject(worldObject);
    }
}
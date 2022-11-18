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

    private void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void Interact() {
        inventory.pickUpText.text = "";
        inventory.pickUpText.enabled = false;

        var terrainTile = GetComponentInParent<GenerateMesh>().gameObject;

        List<WorldObject> worldObjects = worldDataManager.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainTile.name);

        health--;
        
        if (filteredObjects.Count == 0) {
            var tile = gameObject.GetComponentInParent<PlaceObjects>().gameObject;

            foreach (Transform child in tile.transform) {

                var prefabIdentifier = child.gameObject.GetComponent<PrefabIdentifier>();

                if (prefabIdentifier != null) {
                    Harvestable harvestable = child.gameObject.GetComponent<Harvestable>();
                    Interactable interactable = prefabIdentifier.gameObject.GetComponentInChildren<Interactable>();

                    if (harvestable != null) {
                        SaveGameObject(child.gameObject, tile.name, harvestable.health);
                        break;
                    }

                    if (interactable != null) {
                        print(interactable.gameObject.name + " with health " + interactable.health);
                        SaveGameObject(child.gameObject, tile.name, interactable.health);
                        break;
                    }
                }
            }
            
            worldDataManager.SaveData();
        }
        else {
            // Remove current
            WorldObject worldObject = worldObjects.Find(p => p.worldPosition == transform.position);
            worldDataManager.RemoveWorldObject(worldObject);
            Destroy(gameObject);

            // Save
            worldDataManager.SaveData();
        }

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

    private void SaveGameObject(GameObject gameObject, string tileName, int health) {
        PrefabIdentifier prefabIdentifier = gameObject.GetComponentInParent<PrefabIdentifier>();
        WorldObject worldObject = new(prefabIdentifier.prefabIdentifier, tileName, gameObject.transform.position,
            gameObject.transform.rotation, health);
        worldDataManager.AddWorldObject(worldObject);
    }
}
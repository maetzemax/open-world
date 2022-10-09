using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public ItemObject itemObject;
    InventoryManager inventory;

    private void Start() {
        Outline outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;

        inventory = InventoryManager.instance;
    }

    public virtual void Interact() {

        inventory.pickUpText.text = "";
        inventory.pickUpText.enabled = false;

        inventory.AddItem(new ItemObject(itemObject.item, new InventoryObject(itemObject.item.id)));

        Destroy(gameObject);
    }
}

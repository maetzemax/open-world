using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public ItemObject item;
    InventoryManager inventory;

    private void Start() {
        Outline outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;

        inventory = InventoryManager.instance;
    }

    public virtual void Interact() {

        inventory.pickUpText.text = "";
        inventory.pickUpText.enabled = false;

        inventory.AddItem(item);

        Destroy(gameObject);
    }
}

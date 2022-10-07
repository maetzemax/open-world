using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Item item;

    private void Start() {
        Outline outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public virtual void Interact() {

        InventoryManager.instance.pickUpText.text = "";
        InventoryManager.instance.pickUpText.enabled = false;

        InventoryManager.instance.itemList.Add(item);

        Destroy(gameObject);
    }
}

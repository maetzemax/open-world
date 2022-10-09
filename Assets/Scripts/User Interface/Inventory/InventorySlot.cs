using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;

    ItemObject itemObject;

    public void AddItem(ItemObject itemObject) {
        this.itemObject = itemObject;

        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        itemObject = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        InventoryManager.instance.RemoveItem(itemObject);
    }

    public void UseItem() {
        if (itemObject != null) {
            itemObject.item.Use();
        }
    }
}

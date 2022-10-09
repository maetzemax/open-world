using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;

    ItemObject item;

    public void AddItem(ItemObject newItem) {
        item = newItem;

        icon.sprite = item.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        InventoryManager.instance.RemoveItem(item);
    }

    public void UseItem() {
        if (item != null) {
            item.item.Use();
        }
    }
}

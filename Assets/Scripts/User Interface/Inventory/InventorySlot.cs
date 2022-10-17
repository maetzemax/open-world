using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;
    public Button removeButton;
    public Text itemAmount;

    ItemObject itemObject;

    public void AddItem(ItemObject itemObject) {
        this.itemObject = itemObject;

        itemAmount.text = itemObject.item.itemAmount.ToString();
        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        itemObject = null;

        itemAmount.text = "";
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

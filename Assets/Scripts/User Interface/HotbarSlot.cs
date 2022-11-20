using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour {
    public int identifier;
    public Image icon;

    public ItemObject selectedItem = null;

    public void AddItem(ItemObject itemObject) {
        selectedItem = itemObject;

        icon.sprite = itemObject.item.icon;
        icon.enabled = true;
    }

    public void ClearSlot() {
        selectedItem = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour {

    public int identifier;
    public Image icon;

    ItemObject selectedItem;

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

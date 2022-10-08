using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public Text pickUpText;
    public int space = 20;
    public List<Item> itemList;

    void Start() {
        instance = this;
        pickUpText.enabled = false;
    }

    public void AddItem(Item item) {

        if (itemList.Count >= space) {
            return;
        }

        itemList.Add(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveItem(Item item) {
        itemList.Remove(item);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;

    public Text pickUpText;
    public List<Item> itemList;

    void Start() {
        instance = this;
        pickUpText.enabled = false;
    }

}

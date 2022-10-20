using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingPanel : MonoBehaviour {

    public TextMeshProUGUI title;
    public Image icon;

    public TextMeshProUGUI ingredient1;
    public TextMeshProUGUI ingredient2;

    ItemDatabase itemDatabase;

    private void Awake() {
        itemDatabase = ItemDatabase.instance;
    }

    public void AddRecipe(CraftingRecipes craftingRecipe) {
        Item craftingItem = itemDatabase.itemList.Find(i => i.id == craftingRecipe.resultItemID);

        icon.sprite = craftingItem.icon;
        icon.enabled = true;

        title.text = craftingItem.name;

        for (int i = 0; i < craftingRecipe.ingredients.Count; i++) {
            if (i == 0) 
                ingredient1.text = craftingRecipe.ingredients[i].amount + "x " + itemDatabase.itemList.Find(item => item.id == craftingRecipe.ingredients[i].itemID).name;
            if (i == 1)
                ingredient2.text = craftingRecipe.ingredients[i].amount + "x " + itemDatabase.itemList.Find(item => item.id == craftingRecipe.ingredients[i].itemID).name;
        }
    }

}

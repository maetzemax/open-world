using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipes: MonoBehaviour {

    #region Singleton

    public static CraftingRecipes instance;

    void Awake() {

        if (instance != null) {
            Debug.LogWarning("More than one instance CraftingRecipes found");
            return;
        }

        instance = this;
    }

    #endregion

    public readonly List<Ingredients> ingredients;
    public readonly int resultItemID;
    public readonly bool needCraftingBench = true;

    public CraftingRecipes(List<Ingredients> ingredients, int resultItemID, bool needCraftingBench) {
        this.ingredients = ingredients;
        this.resultItemID = resultItemID;
        this.needCraftingBench = needCraftingBench;
    }

    public List<CraftingRecipes> craftingRecipes;

    private void Start() {
        craftingRecipes = new() {
            new(ingredients: new List<Ingredients>() { new(itemID: 2, amount: 16), new(itemID: 3, amount: 8) }, resultItemID: 4, needCraftingBench: true), // Stone Pickaxe
            new(new List<Ingredients>() { new(2, 16), new(1, 8) }, 5, false), // Wood Pickaxe
        };
    }
}

[System.Serializable]
public class Ingredients {
    public int itemID;
    public int amount;

    public Ingredients(int itemID, int amount) {
        this.itemID = itemID;
        this.amount = amount;
    }
}
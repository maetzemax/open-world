using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingRecipe: MonoBehaviour {

    #region Singleton

    public static CraftingRecipe instance;

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
    public readonly int resultAmount = 1;

    public CraftingRecipe(List<Ingredients> ingredients, int resultItemID, bool needCraftingBench, int resultAmount) {
        this.ingredients = ingredients;
        this.resultItemID = resultItemID;
        this.needCraftingBench = needCraftingBench;
        this.resultAmount = resultAmount;
    }

    public List<CraftingRecipe> craftingRecipes;

    private void Start() {
        craftingRecipes = new() {
            new(ingredients: new List<Ingredients>() { new(itemID: 2, amount: 8), new(itemID: 3, amount: 4) }, resultItemID: 4, needCraftingBench: true, resultAmount: 9), // Stone Pickaxe
            new(new List<Ingredients>() { new(2, 8), new(1, 4) }, 5, false, 1), // Wood Pickaxe
            new(new List<Ingredients>() { new(2, 8), new(1, 4) }, 7, false, 1), // Wood Axe
            new(new List<Ingredients>() { new(2, 8), new(1, 4) }, 8, false, 1), // Stone Axe
            new(new List<Ingredients>() { new(2, 8), new(1, 4) }, 9, false, 1), // Iron Pickaxe
            new(new List<Ingredients>() { new(1, 1) }, 2, false, 1), // Stick
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
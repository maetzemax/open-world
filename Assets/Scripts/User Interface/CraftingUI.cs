using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour {

    public GameObject craftingPanel;
    CraftingPanel[] panels;

    List<CraftingRecipe> craftingRecipes;

    private void Awake() {
        craftingRecipes = CraftingRecipe.instance.craftingRecipes;
    }

    private void Start() {
        
        for (int i = 0; i < craftingRecipes.FindAll(cr => !cr.needCraftingBench).Count; i++) {
            Instantiate(craftingPanel, gameObject.transform);
        }
        
        panels = gameObject.GetComponentsInChildren<CraftingPanel>();

        for (int i = 0; i < panels.Length; i++) {
            panels[i].AddRecipe(CraftingRecipe.instance.craftingRecipes.FindAll(cr => !cr.needCraftingBench)[i]);
        }
    }
}

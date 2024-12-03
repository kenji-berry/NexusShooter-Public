using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    const int RECIPE_SIZE = 3;

    public List<Recipe> startingRecipes = new List<Recipe>();
    public RecipeSlot[] recipeSlots = new RecipeSlot[RECIPE_SIZE];
    private Recipe[] recipes = new Recipe[RECIPE_SIZE];

    void Start()
    {
        foreach (Recipe recipe in startingRecipes)
        {
            AddRecipe(recipe);
        }
    }

    void AddRecipe(Recipe recipe)
    {
        int pos = FindNextFreeSlot();

        if (pos == -1)
        {
            return;
        }

        recipes[pos] = recipe;

        recipeSlots[pos].recipe = recipe;
        recipeSlots[pos].UpdateSlot();
    }

    int FindNextFreeSlot()
    {
        for (int i = 0; i < RECIPE_SIZE; i++)
        {
            if (recipes[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}

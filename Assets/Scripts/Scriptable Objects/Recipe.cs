using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe", order = 0)]
public class Recipe : ScriptableObject
{
    [SerializeField]
    [Tooltip("A list of all the ingridients necessary for the result in this recipe.")]
    private List<Ingridient> ingridients = new List<Ingridient>();

    [SerializeField]
    [Tooltip("The resulting drink from the ingridients listed.")]
    private GameObject resultDrink;

    //Getters
    public bool CompareIngridients(List<Ingridient> compareList, out GameObject result) //Returns true if the ingridientlist matches the recipe
    {
        result = null;
        //If they dont match length then its not the correct ingridients
        if(ingridients.Count != compareList.Count) return false;
        for(int i = 0; i < ingridients.Count; i++)
        {
            if (ingridients[i].IngridientID != compareList[i].IngridientID) //If the ingridients are not of the same type
            {
                return false; //Then the lists are not the same
            }
        }
        //If we have made it out of the loop then the lists match
        result = resultDrink;
        return true;
    }
}

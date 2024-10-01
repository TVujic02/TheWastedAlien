using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the base panel layout.")]
    private GameObject baseLayout;

    [SerializeField]
    [Tooltip("A list of the recipe layouts.")]
    private GameObject[] recipeLayouts;

    [SerializeField]
    [Tooltip("The back button for the recipe layouts.")]
    private GameObject backButton;

    [SerializeField]
    [Tooltip("The button that closes the recipe panel.")]
    private GameObject closeButton;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        HideRecipe();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            gameObject.SetActive(false);
        }
    }

    public void ActivatePanel()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
        HideRecipe();
    }

    public void ShowRecipe(int index)
    {
        baseLayout.SetActive(false);
        recipeLayouts[index].SetActive(true);
        backButton.SetActive(true);
        closeButton.SetActive(false);
    }

    public void HideRecipe() 
    {
        closeButton.SetActive(true);
        baseLayout.SetActive(true);
        backButton.SetActive(false);
        foreach(GameObject obj in recipeLayouts) 
        {
            obj.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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
        gameObject.SetActive(true);
    }
}

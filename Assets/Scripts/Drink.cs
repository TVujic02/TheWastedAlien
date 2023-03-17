using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    //Inspector variables
    [SerializeField]
    [Tooltip("A string used to identify this ingridient in recipies.")]
    private string drinkID = string.Empty;

    //Private variables
    private Vector2 mousePosDifference = Vector2.zero;
    private Vector3 startPos = Vector3.zero;

    //Properties
    public string DrinkID { get => drinkID; set => drinkID = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Get the difference
        mousePosDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        startPos = transform.position;
    }
    private void OnMouseDrag()
    {
        //Set the difference
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosDifference;
    }
    private void OnMouseUpAsButton()
    {
        //Reset the position
        transform.position = startPos;
    }
}

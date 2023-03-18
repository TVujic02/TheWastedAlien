using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    //Inspector variables
    [SerializeField]
    [Tooltip("A string used to identify this ingridient in recipies.")]
    private string drinkID = string.Empty;

    [SerializeField]
    [Tooltip("The radius used to check for interactions when the obejct is dropped")]
    private float dropCheckRange = 1f;

    [SerializeField]
    [Tooltip("Reference to the drinkdelivered port.")]
    private DrinkDeliveredPort drinkDeliveredPort;

    [SerializeField]
    [Tooltip("The sprite used when a customer wants this drink.")]
    private Sprite drinkSprite;

    //Private variables
    private Vector2 mousePosDifference = Vector2.zero;
    private Vector3 startPos = Vector3.zero;

    //Properties
    public string DrinkID { get => drinkID; set => drinkID = value; }

    public Sprite GetDrinkSprite => drinkSprite;

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
        //Interaction with customers
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, dropCheckRange);
        Customer customer = null;
        foreach (Collider2D other in colliders) //Look through the colliders for a customer
        {
            customer = other.GetComponent<Customer>();
            if (customer != null) break;
        }
        if (customer != null)
        {
            bool wasServed = customer.ServeCustomer(this); //Serve this customer (yas)
            if(wasServed)
            {
                drinkDeliveredPort.DrinkDelivered?.Invoke(gameObject); //Broadcast that the drink has been delivered
                Destroy(gameObject); //Remove the drink
            }
        }

        //Reset the position
        transform.position = startPos;
    }
}

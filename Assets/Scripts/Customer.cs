using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Customer : MonoBehaviour
{
    //Constants
    private float TARGET_REACHED_THRESHOLD = 0.1f;
    //Inspector variables
    [SerializeField]
    [Tooltip("The movement speed used when this customer is repositioning.")]
    private float moveSpeed = 1f;

    [SerializeField]
    [Tooltip("The ordering data for this customer.")]
    private List<CustomerOrderingData> orderingData = new List<CustomerOrderingData>();

    [SerializeField]
    [Tooltip("The sprite renderer for the drink order.")]
    private SpriteRenderer orderRenderer;

    //Private variables
    private bool correctPosition = false;
    private bool ordering = false;
    private Vector3 targetPosition = Vector3.zero;
    private Drink desiredDrink = null;

    //Properties
    public bool GetIfCorrrectPosition => correctPosition;

    //Events
    [HideInInspector]
    public UnityEvent CustomerServed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Repositioning
        if(!correctPosition)
        {
            Vector3 dir = (targetPosition - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime; //Move with the move speed towards the target
            if(Vector3.Distance(transform.position, targetPosition) < TARGET_REACHED_THRESHOLD) //If we are close enough to our target
            {
                transform.position = targetPosition; //Set the exact position
                targetPosition = Vector3.zero;
                correctPosition = true; //We have reached the target
            }
        }
    }

    public void Order()
    {
        ordering = true;
        if(desiredDrink == null) //If the drink hasnt been set yet
        {
            foreach (CustomerOrderingData data in orderingData)
            {
                float r = Random.Range(0.0f, 1.0f);
                if (r >= data.OrderRate) //If the check is succeded
                {
                    desiredDrink = data.DesiredDrink;
                    break;
                }
                else if(data == orderingData[orderingData.Count-1]) //If its the last data we want that drink to be ordered
                {
                    desiredDrink = data.DesiredDrink;
                }
            }
        }
    }

    public bool ServeCustomer(Drink servingDrink)
    {
        if(ordering && desiredDrink == servingDrink)
        {
            CustomerServed?.Invoke();
            ordering = false;
            desiredDrink = null;
            return true;
        }
        return false;
    }

    public void Reposition(Vector3 newPosition)
    {
        correctPosition = false;
        targetPosition = newPosition;
    }
}

public class CustomerOrderingData
{
    [Tooltip("The drink that is wanted.")]
    public Drink DesiredDrink;

    [Range(0.0f,1.0f)]
    [Tooltip("The chance that this customer will order that drink.")]
    public float OrderRate = 0.5f;
}

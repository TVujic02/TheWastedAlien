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

    //Private variables
    private bool correctPosition = false;
    private bool ordering = false;
    private Vector3 targetPosition = Vector3.zero;

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
    }

    public bool ServeCustomer()
    {
        if(ordering)
        {
            CustomerServed?.Invoke();
            ordering = false;
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

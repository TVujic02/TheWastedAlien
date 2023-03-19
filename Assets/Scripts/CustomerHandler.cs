using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    //Inspector variables
    [Header("Customer Positioning")]
    [SerializeField]
    [Tooltip("The Transform where the customers spawn in.")]
    private Transform customerSpawnPoint;

    [SerializeField]
    [Tooltip("The transform where the customers take there order.")]
    private Transform customerOrderingPoint;

    [SerializeField]
    [Tooltip("The transform where the customer exits.")]
    private Transform customerExitPoint;

    [SerializeField]
    [Tooltip("The amount of distance between all the customers in queue.")]
    private float distanceBetweenCustomers = 1;

    [SerializeField]
    [Tooltip("The max amount of customers allowed in queue.")]
    private int maxCustomers = 8;

    [Header("Customer Spawning")]
    [SerializeField]
    [Tooltip("The minimum and maximum amount of time between the customer spawns.")]
    private Vector2 spawnTimeRange = Vector2.up;

    [SerializeField]
    [NonReorderable]
    [Tooltip("The list of spawnRateDatas for each possible cutsomer spawn.")]
    private List<CustomerSpawnRateData> customerSpawnRateDatas = new List<CustomerSpawnRateData>();

    //Private variables
    private Queue<Customer> customers = new Queue<Customer>();
    private List<Customer> customerExitBuffer = new List<Customer>();
    private float spawnTimer = 0;
    private int customersSpawned = 0;

    //Flags
    private bool repositioning = false;
    private bool routineRunning = false;
    private bool firstRepositioning = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Ordering
        if((repositioning || routineRunning) && firstRepositioning)
        {
            Customer firstCustomer = customers.Peek();

            if(firstCustomer.GetIfCorrrectPosition)
            {
                firstCustomer.Order();
                firstRepositioning = false;
            }
        }

        //Repositioning
        if (repositioning)
        {
            //Check if the customers have reached their targets
            bool allTargetsReached = true;
            foreach (Customer customer in customers)
            {
                if (!customer.GetIfCorrrectPosition) //If this customer does not have the correct position
                {
                    allTargetsReached = false; //All targets are not reached
                    break;
                }
            }
            if (allTargetsReached) //All targets have been reached
            {
                repositioning = false; 
            }
        }
        else if(!routineRunning)
        {
            if (spawnTimer <= 0)
            {
                if(customers.Count < maxCustomers) //Spawn customer if we are within the limit
                    SpawnCustomer();
                spawnTimer = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y); //Set timer
            }
            else
                spawnTimer -= Time.deltaTime;

        }

        for(int i = customerExitBuffer.Count - 1; i >= 0;i--) //Check the buffer
        {
            if (customerExitBuffer[i].GetIfCorrrectPosition) //Remove those who have reached the end position
            {
                Destroy(customerExitBuffer[i].gameObject);
                customerExitBuffer.RemoveAt(i);
            }
        }
    }

    private void SpawnCustomer()
    {
        foreach(CustomerSpawnRateData data in customerSpawnRateDatas)
        {
            float r = UnityEngine.Random.Range(0.0f, 1.0f); //Get a random float used to detirmine if this customer should be spawned
            if (r <= data.BaseRate || data == customerSpawnRateDatas[customerSpawnRateDatas.Count - 1]) //If we succeded the check or if its the last data
            {
                GameObject obj = Instantiate(data.customerPrefab, customerSpawnPoint.position, Quaternion.identity); //Spawn at spawnPoint
                Customer newCustomer = obj.GetComponent<Customer>();
                if (customers.Count == 0) firstRepositioning = true; //Is this new customer the first one in queue
                customers.Enqueue(newCustomer); //Add it to the queue
                newCustomer.CustomerServed.AddListener(OnCustomerServed);
                newCustomer.Reposition(customerOrderingPoint.position + (Vector3.left * distanceBetweenCustomers * (customers.Count - 1))); //Reposition it from the spawnpoint to the correct position
                repositioning = true;
                customersSpawned++; //Increase counter
                break;
            }
        }
    }

    private void OnCustomerServed()
    {
        //Remove the current first customer
        Customer servedCustomer = customers.Dequeue(); //Dequeue the customer
        servedCustomer.Reposition(customerExitPoint.position); //Position it towards the exit
        customerExitBuffer.Add(servedCustomer); //Add it to the buffer so we can remove it when it has reached the exit point

        StartCoroutine("RepositioningRoutine");
        routineRunning = true;
    }

    private IEnumerator RepositioningRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        firstRepositioning = true;

        //Start repositioning of the other customers
        int index = 0;
        foreach (Customer customer in customers)
        {
            Vector3 newPosition = customerOrderingPoint.position + (Vector3.left * distanceBetweenCustomers * index);
            customer.Reposition(newPosition);
            index++;
            yield return new WaitForSeconds(0.5f);
        }
        repositioning = true;
        routineRunning = false;

    }
}

[Serializable]
public class CustomerSpawnRateData
{
    [Header("Customer")]
    public GameObject customerPrefab;

    [Header("BaseRate")]
    [Range(0.0f, 1.0f)]
    [Tooltip("The base spawnrate for this customer.")]
    public float BaseRate = 0.5f;
     /*
    [Tooltip("The interval where only the BaseSpawnRate is used.")]
    public Vector2Int BaseRange = Vector2Int.zero;

    [Header("ModifiedRate")]
    [Range(0.0f,1.0f)]
    [Tooltip("The fully modified spawnrate for this customer.")]
    public float ModifiedRate = 1.0f;

    [Tooltip("The interval where the base spawnrate is modified towards the modified spawnrate (on max it has reached the full modification).")]
    public Vector2Int ModifiedRange = Vector2Int.zero;
     */
}

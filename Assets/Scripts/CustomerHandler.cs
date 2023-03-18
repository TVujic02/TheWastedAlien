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
    private bool repositioning = false;
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    // Update is called once per frame
    void Update()
    {
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
                customers.Peek().Order(); //The first customer in the queue starts their order
            }
        }
        else
        {
            if (spawnTimer <= 0)
            {
                SpawnCustomer();
                spawnTimer = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y);
            }
            else
                spawnTimer -= Time.deltaTime;

        }

        for(int i = customerExitBuffer.Count- 1; i >= 0;i--) //Check the buffer
        {
            if (customerExitBuffer[i].GetIfCorrrectPosition) //Remove those who have reached the end position
            {
                customerExitBuffer.RemoveAt(i);
            }
        }
    }

    private void SpawnCustomer()
    {
        foreach(CustomerSpawnRateData data in customerSpawnRateDatas)
        {
            if (customersSpawned >= data.BaseRange.x && customersSpawned < data.BaseRange.y) //If the customer count is within the base range
            {
                float r = UnityEngine.Random.Range(0.0f, 1.0f); //Get a random float used to detirmine if this customer should be spawned
                if (r >= data.BaseRate) //If we succeded the check
                {
                    Debug.Log("Spawn");
                    GameObject obj = Instantiate(data.customerPrefab, customerSpawnPoint.position, Quaternion.identity); //Spawn at spawnPoint
                    Customer newCustomer = obj.GetComponent<Customer>();
                    customers.Enqueue(newCustomer); //Add it to the queue
                    newCustomer.CustomerServed.AddListener(OnCustomerServed);
                    newCustomer.Reposition(customerOrderingPoint.position + (Vector3.left * distanceBetweenCustomers * (customers.Count - 1))); //Reposition it from the spawnpoint to the correct position
                    customersSpawned++; //Increase counter
                    break;
                }
            }
        }
    }

    private void OnCustomerServed()
    {
        //Remove the current first customer
        Customer servedCustomer = customers.Dequeue(); //Dequeue the customer
        servedCustomer.Reposition(customerExitPoint.position); //Position it towards the exit
        customerExitBuffer.Add(servedCustomer); //Add it to the buffer so we can remove it when it has reached the exit point

        //Start repositioning of the other customers
        int index = 0;
        foreach(Customer customer in customers)
        {
            Vector3 newPosition = customerOrderingPoint.position + (Vector3.left * distanceBetweenCustomers * index);
            customer.Reposition(newPosition);
            index++;
        }
        repositioning = true;
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

    [Tooltip("The interval where only the BaseSpawnRate is used.")]
    public Vector2Int BaseRange = Vector2Int.zero;

    [Tooltip("After this many customers spawned this customer will no longer spawn.")]
    public int StopCount = 100;

    [Header("ModifiedRate")]
    [Range(0.0f,1.0f)]
    [Tooltip("The fully modified spawnrate for this customer.")]
    public float ModifiedRate = 1.0f;

    [Tooltip("The interval where the base spawnrate is modified towards the modified spawnrate (on max it has reached the full modification).")]
    public Vector2Int ModifiedRange = Vector2Int.zero; 
}

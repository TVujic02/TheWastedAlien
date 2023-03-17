using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    //Inspector variables
    [Header("Transforms")]
    [SerializeField]
    [Tooltip("The Transform where the customers spawn in.")]
    private Transform customerSpawnPoint;

    [SerializeField]
    [Tooltip("The transform where the customers take there order.")]
    private Transform customerOrderingPoint;

    [SerializeField]
    [Tooltip("The transform where the customer exits.")]
    private Transform customerExitPoint;

    [Header("Customer Spawning")]
    [SerializeField]
    [Tooltip("The minimum and maximum amount of time between the customer spawns.")]
    private Vector2 spawnTimeRange = Vector2.up;

    [SerializeField]
    [NonReorderable]
    [Tooltip("The list of spawnRateDatas for each possible cutsomer spawn.")]
    private List<CustomerSpawnRateData> customerSpawnRateDatas = new List<CustomerSpawnRateData>();

    //Private variables

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

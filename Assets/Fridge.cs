using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The sidebar used to control the different ingridient sets.")]
    private GameObject sidebar;

    [SerializeField]
    [Tooltip("A list of all the diferent ingridients sets.")]
    private List<GameObject> ingridientSets = new List<GameObject>();

    [SerializeField]
    [Tooltip("The point where all the ingridents are spawned.")]
    private Transform ingridientSpawnPoint;

    private int currentSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        sidebar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        sidebar.SetActive(true);
        ActivateSet(currentSet);
    }

    private void OnMouseExit()
    {
        sidebar.SetActive(false);
        ActivateSet(-1); //No set has this index
    }

    public void ActivateSet(int index)
    {
        for(int i = 0; i < ingridientSets.Count; i++) 
        {
            if(i == index)
            {
                ingridientSets[i].SetActive(true); //Activate index
            }
            else
            {
                ingridientSets[i].SetActive(false); //Deactivate the rest
            }
        }
    }

    public void SpawnIngridient(GameObject ingridient)
    {
        Instantiate(ingridient, ingridientSpawnPoint.position, Quaternion.identity);
    }
}

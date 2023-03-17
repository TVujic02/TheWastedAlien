using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkStand : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The max amount of drinks that fit on the stand.")]
    private int maxDrinks = 3;

    [SerializeField]
    [Tooltip("The distance between each drink on the stand.")]
    private float drinkDistance = 1.0f;

    [SerializeField]
    [Tooltip("Reference to the point where the drink row starts.")]
    private Transform drinkStandStart;

    [SerializeField]
    [Tooltip("The speed at wich the drink reorganise after a new drink has been added.")]
    private float organiseSpeed = 1f;

    [SerializeField]
    [Tooltip("Reference to the drinkdelivered port.")]
    private DrinkDeliveredPort drinkDeliveredPort;

    //Private variables
    private List<GameObject> drinkRow = new List<GameObject>();
    private bool organiseLerp = false;
    private float t = 0f;
    private List<Vector3> drinkStartPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        drinkDeliveredPort.DrinkDelivered.AddListener(RemoveDrink);
    }

    // Update is called once per frame
    void Update()
    {
        if(organiseLerp) //Lerp the drink positions
        {
            //Help vars
            GameObject drink = null;
            Vector3 endPos = Vector3.zero;

            t += Time.deltaTime * organiseSpeed; //Increase with time

            //If t is more than one we can stop lerping
            if (t > 1)
            {
                organiseLerp = false;
                t = 0; //Reset t
            }
            else
            {
                //Apply lerp
                for (int i = 0; i < drinkRow.Count; i++)
                {
                    drink = drinkRow[i];
                    endPos = drinkStandStart.position + (Vector3.right * i * drinkDistance); //The end pos is the correct position of the drink
                    drink.transform.position = Vector3.Lerp(drinkStartPositions[i], endPos, t);
                }
            }
            
        }
    }

    public void AddDrink(GameObject drink)
    {
        drinkRow.Add(drink);
        drink.transform.position = drinkStandStart.position + (Vector3.right * (drinkRow.Count - 1) * drinkDistance); //Position it last in the row
    }

    private void RemoveDrink(GameObject drink)
    {
        drinkRow.Remove(drink);
        OrganiseDrinks();
    }

    private void OrganiseDrinks()
    {
        organiseLerp = true;
    }
}

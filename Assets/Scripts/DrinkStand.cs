using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkStand : MonoBehaviour
{
    //Constants
    private const float ORGANISE_SPEED = 1.5f;

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
    [Tooltip("Reference to the drinkdelivered port.")]
    private DrinkDestroyedPort drinkDeliveredPort;

    [SerializeField]
    [Tooltip("Reference to the system that is played when a drink is added (no looping).")]
    private ParticleSystem drinkAddedSystem;

    //Private variables
    private List<GameObject> drinkRow = new List<GameObject>();
    private bool organiseLerp = false;
    private float t = 0f;
    private List<Vector3> drinkStartPositions = new List<Vector3>();

    public bool CanAddDrink => drinkRow.Count < maxDrinks;

    // Start is called before the first frame update
    void Start()
    {
        drinkDeliveredPort.DrinkDestroyed.AddListener(RemoveDrink);
    }

    // Update is called once per frame
    void Update()
    {
        if(organiseLerp) //Lerp the drink positions
        {
            //Help vars
            GameObject drink = null;
            Vector3 endPos = Vector3.zero;

            t += Time.deltaTime * ORGANISE_SPEED; //Increase with time

            //If t is more than one we can stop lerping
            if (t >= 1)
            {
                organiseLerp = false;
                t = 0; //Reset t
                for(int i = 0; i < drinkRow.Count; i++)
                {
                    drinkStartPositions[i] = drinkRow[i].transform.position; //Set start positions
                }
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
        if (drinkRow.Count >= maxDrinks)
            return;
        drinkRow.Add(drink);
        drink.transform.position = drinkStandStart.position + (Vector3.right * (drinkRow.Count - 1) * drinkDistance); //Position it last in the row
        drinkStartPositions.Add(drink.transform.position);

        //Play sytem
        if(drinkAddedSystem != null)
        {
            drinkAddedSystem.Play();
        }
    }

    private void RemoveDrink(GameObject drink)
    {
        drinkStartPositions.RemoveAt(drinkRow.IndexOf(drink));
        drinkRow.Remove(drink);
        OrganiseDrinks();
    }

    private void OrganiseDrinks()
    {
        organiseLerp = true;
    }
}

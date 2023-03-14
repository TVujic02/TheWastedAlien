using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrinkMixer : MonoBehaviour, IUtensil
{
    //Constants
    private const float PREVIOUS_TIMER_MAX = 0.4f;
    private const int MIN_INGRIDIENT_AMOUNT = 2;

    //Inspector variables
    [SerializeField]
    [Tooltip("The max amount of ingridients that can be stored.")]
    private int maxIngridientAmount = 3;

    [SerializeField]
    [Tooltip("The amount of movements required to shake the mixer")]
    private float shakeThreshold = 0.1f;

    [SerializeField]
    [Tooltip("The amount of time needed to complete the shake.")]
    private float shakeTime = 2f;

    //Private variable
    private List<Ingridient> storedIngridients = new List<Ingridient>();
    private Vector2 mousePosDifference = Vector2.zero;
    private Vector2 previousPos, storedPos;
    private float previousTimer = PREVIOUS_TIMER_MAX;
    private float shakeTimer = 0;
    private bool shaking = false;

    // Start is called before the first frame update
    void Start()
    {
        storedPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousTimer <= 0)
        {
            previousPos = storedPos; //Delayed previous pos
            storedPos = transform.position;
            //Shaking
            if (Vector2.Distance((Vector2)transform.position, previousPos) > shakeThreshold) //If the distance moved is bigger we are shaking the mixer
            {
                shaking = true;
            }
            else
            {
                shaking = false; //The mixer isnt being shaked enough
            }
            previousTimer = PREVIOUS_TIMER_MAX;
        }
        else
            previousTimer -= Time.deltaTime;

        //Shake timer
        if (shaking)
            shakeTimer += Time.deltaTime;
        else
            shakeTimer = 0;

        //Check the shaketime
        if (shakeTimer >= shakeTime)
        {
            //Shake is done
            if (storedIngridients.Count >= MIN_INGRIDIENT_AMOUNT) //If we have more than one ingridients we can mix something
            {
                Mix(); //Call the mix function
            }
            shakeTimer = 0;
        }
    }

    private void OnMouseDown()
    {
        //Get the difference
        mousePosDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }
    private void OnMouseDrag()
    {
        //Set the difference
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosDifference;
    }

    public void IngridientInteraction(Ingridient ingridient)
    {
        if (storedIngridients.Count >= maxIngridientAmount) //Caps the amount of ingridients
            return;
        //Store it in the scene
        ingridient.gameObject.SetActive(false);
        ingridient.transform.SetParent(transform);

        //Store it in this class
        storedIngridients.Add(ingridient);
    }

    private void Mix()
    {
        //Implement mix logic
        foreach(Ingridient ingridient in storedIngridients)
        {
            ingridient.gameObject.SetActive(true); //Release the ingridient
            ingridient.transform.SetParent(null);
        }
        storedIngridients.Clear(); //Remove all ingridients from the list
    }
}

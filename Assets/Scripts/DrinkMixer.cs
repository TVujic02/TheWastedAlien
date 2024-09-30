using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkMixer : MonoBehaviour, IUtensil
{
    //Constants
    private const float PREVIOUS_TIMER_MAX = 0.025f;
    private const int MIN_INGRIDIENT_AMOUNT = 2;

    //Inspector variables
    [Header("Shaker Values")]
    [SerializeField]
    [Tooltip("The max amount of ingridients that can be stored.")]
    private int maxIngridientAmount = 3;

    [SerializeField]
    [Tooltip("The amount of movements required to shake the mixer")]
    private float shakeStartThreshold = 0.1f;

    [SerializeField]
    [Tooltip("The amount of movements required to shake the mixer")]
    private float shakeEndThreshold = 0.1f;

    [SerializeField]
    [Tooltip("The amount of time needed to complete the shake.")]
    private float shakeTime = 2f;

    [SerializeField]
    [Tooltip("The different recipes that ur able to make with this mixer.")]
    private List<Recipe> recipes = new List<Recipe>();

    [SerializeField]
    [Tooltip("The drink that is made if there is no matching recipe.")]
    private GameObject defaultDrink;

    [Header("References")]
    [SerializeField]
    [Tooltip("Reference to the drink stand.")]
    private DrinkStand drinkStand;
    [SerializeField]
    [Tooltip("Reference to the drink show port.")]
    private DrinkShowPort drinkShowPort;

    [SerializeField]
    [Tooltip("Refrence to the Mixer Indicator.")]
    private MixerIndicator mixerIndicator;

    [Header("Audio")]
    [SerializeField]
    [Tooltip("Reference to the audio source used for the shaker sounds.")]
    private AudioSource shakerSource;

    [SerializeField]
    [Tooltip("Reference to the exit clip of the shaker.")]
    private AudioClip[] shakerExitClips;

    //Private variable
    private List<Ingridient> storedIngridients = new List<Ingridient>();
    private Vector2 mousePosDifference = Vector2.zero;
    private Vector2 previousPos, storedPos;
    private float previousTimer = PREVIOUS_TIMER_MAX;
    private float shakeTimer = 0;
    private bool shaking = false;
    private bool playedExitSound = false;
    private GameObject storedDrink = null;

    // Start is called before the first frame update
    void Start()
    {
        storedPos = transform.position;
        drinkShowPort.DrinkShowEnd.AddListener(OnShowEnded);
        mixerIndicator.UpdateIndicator(storedIngridients); //Update indicator
    }

    // Update is called once per frame
    void Update()
    {
        if (previousTimer <= 0)
        {
            previousPos = storedPos; //Delayed previous pos
            storedPos = transform.position;
            //Shaking
            if (Vector2.Distance((Vector2)transform.position, previousPos) > shakeStartThreshold && !shaking) //If the distance moved is bigger we start shaking the mixer
            {
                shaking = true;
                //Shaking audio
                if(shakerSource != null && !shakerSource.isPlaying) 
                {
                    shakerSource.Play();
                    playedExitSound = false;
                }
            }
            else if(Vector2.Distance((Vector2)transform.position, previousPos) < shakeEndThreshold) //If its below the end threshold we stop shaking
            {
                shaking = false; //The mixer isnt being shaked enough
                if (shakerSource.isPlaying && !playedExitSound)
                {
                    //Play shaker exit audio
                    shakerSource.Stop();
                    shakerSource.PlayOneShot(shakerExitClips[Random.Range(0, shakerExitClips.Length-1)]); //Play exit clip as one shot
                    playedExitSound = true;
                }
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
        mixerIndicator.UpdateIndicator(storedIngridients); //Update indicator
    }

    private void Mix()
    {
        //If there is already a drink stored we dont mix a new one
        if (storedDrink != null)
            return;

        GameObject result = null;
        GameObject instance = null;
        bool canAddDrink = false;
        foreach (Recipe recipe in recipes)
        {
            if(recipe.CompareIngridients(storedIngridients, out result)) //Returns true if we matched the recipe
            {
                instance = Instantiate(result, transform.position, transform.rotation); //Instatiate the drink created by the recipe
                canAddDrink = drinkStand.CanAddDrink; //See if we can add the drink
                if (canAddDrink) //If there was enough space on the stand
                {
                    foreach (Ingridient ingridient in storedIngridients) //Remove stored ingridients
                    {
                        Destroy(ingridient.gameObject);
                    }
                    storedIngridients.Clear();
                    mixerIndicator.UpdateIndicator(storedIngridients); //Update indicator

                    drinkShowPort.DrinkShowStart?.Invoke(instance);
                    storedDrink = instance; //Store the drink
                    storedDrink.SetActive(false);
                }
                else //If there wasnt enough space on the stand
                {
                    Destroy(instance);
                    ReleaseIngridients();
                }
                return; //Exit out of the function
            }
        }
        //If we havent exited yet there was no matching recipes so we should create the default drink
        instance = Instantiate(defaultDrink, transform.position, transform.rotation); //Instatiate the drink created by the recipe
        canAddDrink = drinkStand.CanAddDrink; //See if we can add the drink
        if (canAddDrink) //If there was enough space on the stand
        {
            foreach (Ingridient ingridient in storedIngridients) //Remove stored ingridients
            {
                Destroy(ingridient.gameObject);
            }
            storedIngridients.Clear();
            mixerIndicator.UpdateIndicator(storedIngridients); //Update indicator

            drinkShowPort.DrinkShowStart?.Invoke(instance);
            storedDrink = instance; //Store the drink
            storedDrink.SetActive(false);
        }
        else //If there wasnt enough space on the stand
        {
            Destroy(instance);
            ReleaseIngridients();
        }
    }

    public void ReleaseIngridients()
    {
        foreach (Ingridient ingridient in storedIngridients)
        {
            ingridient.gameObject.SetActive(true); //Release the ingridient
            ingridient.transform.SetParent(null);
        }
        storedIngridients.Clear(); //Remove all ingridients from the list
        mixerIndicator.UpdateIndicator(storedIngridients); //Update indicator
    }

    private void OnShowEnded()
    {
        storedDrink.SetActive(true);
        drinkStand.AddDrink(storedDrink);
        storedDrink = null;
    }
}

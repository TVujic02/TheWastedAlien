using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrinkShow : MonoBehaviour
{
    [Header("Fading")]
    [SerializeField]
    [Tooltip("Reference to all the Sprite renderers that should fade")]
    private SpriteRenderer[] fadingRenderers;

    [SerializeField]
    [Tooltip("Referemce to the drink sprite renderer.")]
    private SpriteRenderer drinkSpriteRenderer;

    [SerializeField]
    [Tooltip("The text object that displays the name of the drink.")]
    private TextMeshProUGUI nameText;
    [SerializeField]
    [Tooltip("The text object that displays the description of the drink.")]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    [Tooltip("The speed that the renderers are faded in and out.")]
    private float fadeSpeed = 3f;

    [Header("Other")]
    [SerializeField]
    [Tooltip("Reference to the drink show port.")]
    private DrinkShowPort drinkShowPort;

    [SerializeField]
    [Tooltip("Reference to the audio clipped that is placed when the drink is shown.")]
    private AudioClip drinkShowClip;

    private bool fadingIn = false, fadingOut;
    private float fade = 0;
    private Dictionary<SpriteRenderer, float> maxOpacity = new Dictionary<SpriteRenderer, float>();
    private AudioSource drinkShowSource;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set alpha values
        foreach (SpriteRenderer renderer in fadingRenderers)
        {
            maxOpacity.Add(renderer, renderer.color.a);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, fade);
        }
        nameText.alpha = fade;
        descriptionText.alpha = fade;

        gameObject.SetActive(false);
        drinkShowPort.DrinkShowStart.AddListener(StartDrinkShow);
        drinkShowSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Fade back out
        if(Input.GetMouseButtonDown(0) && !fadingIn)
        {
            fadingOut = true;
            fade = 1;
        }
        //Fading in
        if(fadingIn)
        {
            if(fade >= 1)
            {
                fadingIn = false;
                fade = 1;
                //Play show sound
                if(drinkShowSource != null && drinkShowClip!= null) 
                {
                    drinkShowSource.PlayOneShot(drinkShowClip);
                }
            }
            else
            {
                fade += Time.deltaTime * fadeSpeed;
                //Set alpha values
                foreach(SpriteRenderer renderer in fadingRenderers)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(0, maxOpacity[renderer], fade));
                }
                nameText.alpha = fade;
                descriptionText.alpha = fade;
            }
        }

        //Fading out
        if (fadingOut)
        {
            if (fade <= 0)
            {
                fadingOut = false;
                fade = 0;
                gameObject.SetActive(false);
                drinkSpriteRenderer.sprite = null;
                drinkShowPort.DrinkShowEnd?.Invoke();
            }
            else
            {
                fade -= Time.deltaTime * fadeSpeed;
                //Set alpha values
                foreach (SpriteRenderer renderer in fadingRenderers)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(0, maxOpacity[renderer], fade));
                }
                nameText.alpha = fade;
                descriptionText.alpha = fade;
            }
        }
    }

    public void StartDrinkShow(GameObject drink)
    {
        Drink drinkScript = drink.GetComponent<Drink>();
        Sprite drinkSprite = drinkScript.GetDrinkSprite;
        gameObject.SetActive(true);
        fadingIn = true;
        drinkSpriteRenderer.sprite = drinkSprite;
        nameText.text = drinkScript.DrinkID;
        descriptionText.text = drinkScript.DrinkDescription;
        fade = 0;
    }
}

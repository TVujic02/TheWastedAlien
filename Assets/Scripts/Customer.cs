using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Customer : MonoBehaviour
{
    //Constants
    private float TARGET_REACHED_THRESHOLD = 0.1f;
    //Inspector variables
    [SerializeField]
    [Tooltip("The movement speed used when this customer is repositioning.")]
    private float moveSpeed = 1f;

    [SerializeField]
    [Tooltip("The amount that the customer bobs when walking.")]
    private float walkBobAmount = 0.1f;

    [SerializeField]
    [Tooltip("The speed at wich the customer bobs up and down.")]
    private float walkBobSpeed = 3f;

    [SerializeField]
    [Tooltip("The amount that the customer bobs when walking.")]
    private float idleBobAmount = 0.05f;

    [SerializeField]
    [Tooltip("The speed at wich the customer bobs up and down.")]
    private float idleBobSpeed = 3f;

    [SerializeField]
    [NonReorderable]
    [Tooltip("The ordering data for this customer.")]
    private List<CustomerOrderingData> orderingData = new List<CustomerOrderingData>();

    [SerializeField]
    [Tooltip("The text object used to order the drink.")]
    private TextMeshProUGUI orderText;

    [SerializeField]
    [Tooltip("The sprite renderer of the chat bubble.")]
    private SpriteRenderer chatBubbleRenderer;

    [SerializeField]
    [Tooltip("The speed that the bubble fades.")]
    private float fadeSpeed = 1f;

    //Private variables
    private bool correctPosition = false;
    private bool ordering = false;
    private Vector3 targetPosition = Vector3.zero;
    private string desiredDrink = string.Empty;
    private float bobPos = 0;
    private float baseY = 0;
    private float fade = 0;
    private bool fadingIn = false;
    private bool fadingOut = false;

    //Properties
    public bool GetIfCorrrectPosition => correctPosition;

    //Events
    [HideInInspector]
    public UnityEvent CustomerServed;
    // Start is called before the first frame update
    void Start()
    {
        orderText.text = string.Empty;
        baseY = transform.position.y;
        chatBubbleRenderer.color = new Color(chatBubbleRenderer.color.r, chatBubbleRenderer.color.g, chatBubbleRenderer.color.b, fade);
        orderText.alpha = fade;
    }

    // Update is called once per frame
    void Update()
    {
        //Repositioning
        if (!correctPosition)
        {
            //Movement
            Vector3 dir = (targetPosition - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime; //Move with the move speed towards the target

            //Bobbing
            transform.position = new Vector3(transform.position.x, baseY + Mathf.Sin(bobPos) * walkBobAmount);
            bobPos += Time.deltaTime * walkBobSpeed;

            if (Vector3.Distance(transform.position, targetPosition) < TARGET_REACHED_THRESHOLD) //If we are close enough to our target
            {
                transform.position = targetPosition; //Set the exact position
                targetPosition = Vector3.zero;
                correctPosition = true; //We have reached the target
            }
        }

        //Idle bob
        if(correctPosition)
        {
            transform.position = new Vector3(transform.position.x, baseY + Mathf.Sin(bobPos) * idleBobAmount);
            bobPos += Time.deltaTime * idleBobSpeed;
        }

        //Fading chat bubble
        if(fadingIn)
        {
            if (fade >= 1)
            {
                fade = 1;
                fadingIn = false;
            }
            else
            {
                fade += Time.deltaTime * fadeSpeed;
                chatBubbleRenderer.color = new Color(chatBubbleRenderer.color.r, chatBubbleRenderer.color.g, chatBubbleRenderer.color.b, fade);
                orderText.alpha = fade;
            }
        }

        if (fadingOut)
        {
            if (fade <= 0)
            {
                fade = 0;
                fadingOut = false;
                orderText.text = string.Empty;
            }
            else
            {
                fade -= Time.deltaTime * fadeSpeed;
                chatBubbleRenderer.color = new Color(chatBubbleRenderer.color.r, chatBubbleRenderer.color.g, chatBubbleRenderer.color.b, fade);
                orderText.alpha = fade;
            }
        }
    }

    public void Order()
    {
        ordering = true;
        if(desiredDrink == string.Empty) //If the drink hasnt been set yet
        {
            foreach (CustomerOrderingData data in orderingData)
            {
                float r = UnityEngine.Random.Range(0.0f, 1.0f);
                if (r <= data.OrderRate) //If the check is succeded
                {
                    desiredDrink = data.DesiredDrink.DrinkID;
                    orderText.text = data.OrderText;
                    fadingIn = true;
                    break;
                }
                else if(data == orderingData[orderingData.Count-1]) //If its the last data we want that drink to be ordered
                {
                    desiredDrink = data.DesiredDrink.DrinkID;
                    orderText.text = data.OrderText;
                    fadingIn = true;
                }
            }
        }
    }

    public bool ServeCustomer(Drink servingDrink)
    {
        if(ordering && desiredDrink == servingDrink.DrinkID)
        {
            CustomerServed?.Invoke();
            ordering = false;
            fadingOut = true;
            fadingIn = false;
            desiredDrink = string.Empty;
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
[Serializable]
public class CustomerOrderingData
{
    [Tooltip("The drink that is wanted.")]
    public Drink DesiredDrink;

    [Range(0.0f,1.0f)]
    [Tooltip("The chance that this customer will order that drink.")]
    public float OrderRate = 0.5f;

    [Tooltip("The string that is shown when the customer.")]
    public string OrderText = string.Empty;
}

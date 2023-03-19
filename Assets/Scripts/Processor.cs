using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ProcessinType {Crush, Cut, Fry}
public class Processor : MonoBehaviour, IUtensil
{
    //Inspector variables
    [SerializeField]
    [Tooltip("The type of processing this processor preforms.")]
    private ProcessinType processingType = ProcessinType.Crush;

    [SerializeField]
    [Tooltip("The audio clipped played when something is processed.")]
    private AudioClip processingClip;

    [SerializeField]
    [Tooltip("The particle system that is played when smoething is processed.")]
    private ParticleSystem processingSystem;

    //Private variables
    private AudioSource processorSource;

    // Start is called before the first frame update
    void Start()
    {
        processorSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IngridientInteraction(Ingridient ingridient)
    {
        GameObject result = null;
        //Check for possible processing results
        switch(processingType)
        {
            case ProcessinType.Crush:
                if (ingridient.Crushable) result = ingridient.CrushResult;
                break;
            case ProcessinType.Cut:
                if (ingridient.Cutable) result = ingridient.CutResult;
                break;
            case ProcessinType.Fry:
                if (ingridient.Fryable) result = ingridient.FryResult;
                break;
        }
        if(result != null) //If we have a result
        {
            //Remove the ingridient and replace it with its processed version
            Instantiate(result, transform.position, Quaternion.identity);
            Destroy(ingridient.gameObject);

            //Play audio
            if(processorSource != null && processingClip != null) 
            {
                processorSource.PlayOneShot(processingClip);
            }

            //Play particle system
            if(processingSystem!= null) 
            {
                processingSystem.Play();
            }
        }
    }
}

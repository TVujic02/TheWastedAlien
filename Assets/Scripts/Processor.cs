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

    //Private variables

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IngridientInteraction(Ingridient ingridient)
    {
        GameObject result = null;
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
        if(result != null)
        {
            Instantiate(result, transform.position, Quaternion.identity);
            Destroy(ingridient.gameObject);
        }
    }
}

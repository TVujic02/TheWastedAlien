using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour, IUtensil
{
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
        Destroy(ingridient.gameObject);
    }
}

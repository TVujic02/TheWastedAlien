using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSceneButton : MonoBehaviour
{
    public void ClearScene()
    {
        Ingridient[] ingridientsInScene = FindObjectsOfType<Ingridient>();

        foreach(Ingridient ingridient in ingridientsInScene) 
        {
            Destroy(ingridient.gameObject);
        }
    }
}

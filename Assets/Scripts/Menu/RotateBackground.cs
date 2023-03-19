using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBackground : MonoBehaviour
{
 
    [SerializeField] float rotationSpeed = 1f; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}

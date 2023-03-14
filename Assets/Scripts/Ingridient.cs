using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ingridient : MonoBehaviour
{
    //Inspector variables
    [Header("IngridientData")]
    [SerializeField]
    [Tooltip("A string used to identify this ingridient in recipies.")]
    private string ingridientID = string.Empty;

    [Header("Physics")]
    [SerializeField]
    [Tooltip("The force used to simulate throwing. (applied in the moving direction when the object is released)")]
    private float releaseForce = 500f;

    [SerializeField]
    [Tooltip("The radius used to check for interactions when the obejct is dropeed")]
    private float dropCheckRange = 1f;

    [HideInInspector]
    public bool Crushable, Cutable, Fryable;

    [HideInInspector]
    public GameObject CrushResult, CutResult, FryResult;

    //Private variables
    private Vector2 mousePosDifference = Vector2.zero;
    private Rigidbody2D rb;
    private Vector2 previousPos, storedPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        storedPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        previousPos = storedPos; //Delayed previous pos
        storedPos = transform.position;
    }

    private void OnMouseDown()
    {
        //Get the difference
        mousePosDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        previousPos = transform.position;
    }
    private void OnMouseDrag()
    {
        //Set the difference
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePosDifference;
        rb.velocity = Vector2.zero; //Make sure gravity isnt building up velocity
    }

    private void OnMouseUpAsButton()
    {
        //When we stop dragging this object
        rb.AddForce(((Vector2)transform.position - previousPos).normalized * releaseForce); //Force that simulates throwing

        //Interaction with utensils
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, dropCheckRange);
        IUtensil utensil = null;
        foreach (Collider2D other in colliders) //Look through the colliders for a processor
        {
            utensil = other.GetComponent<IUtensil>();
            if (utensil != null) break;
        }
        if(utensil != null) 
        {
            utensil.IngridientInteraction(this); //Process
        }
    }
}

[CustomEditor(typeof(Ingridient))]
public class ProcessingData_Editor : Editor
{
    bool showProcessingData = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Ingridient ingridient = (Ingridient)target;
        
        showProcessingData = EditorGUILayout.Foldout(showProcessingData, "Processing Data");

        if (showProcessingData)
        {
            //Processing Properties
            var crushableProperty = serializedObject.FindProperty("Crushable");
            var cutableProperty = serializedObject.FindProperty("Cutable");
            var fryableProperty = serializedObject.FindProperty("Fryable");
            var crushResultProprety = serializedObject.FindProperty("CrushResult");
            var cutResultProperty = serializedObject.FindProperty("CutResult");
            var fryResultProprety = serializedObject.FindProperty("FryResult");
            

            EditorGUILayout.PropertyField(crushableProperty);
            EditorGUILayout.PropertyField(cutableProperty);
            EditorGUILayout.PropertyField(fryableProperty);

            //Results
            if (ingridient.Crushable)
            {
                EditorGUILayout.PropertyField(crushResultProprety);
            }
            if (ingridient.Cutable)
            {
                EditorGUILayout.PropertyField(cutResultProperty);
            }
            if (ingridient.Fryable)
            {
                EditorGUILayout.PropertyField(fryResultProprety);
            }
        }
        serializedObject.ApplyModifiedProperties();

    }
}

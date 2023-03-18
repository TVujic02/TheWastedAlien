using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DrinkDestroyedPort", menuName = "Ports/DrinkDestroyedPort", order = 0)]
public class DrinkDestroyedPort : ScriptableObject
{
    [HideInInspector]
    public UnityEvent<GameObject> DrinkDestroyed;
}

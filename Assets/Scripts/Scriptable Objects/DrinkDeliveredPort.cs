using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DrinkDeliveredPort", menuName = "Ports/DrinkDeliveredPort", order = 0)]
public class DrinkDeliveredPort : ScriptableObject
{
    public UnityEvent<GameObject> DrinkDelivered;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DrinkShowPort", menuName = "Ports/DrinkShowPort", order = 1)]
public class DrinkShowPort : ScriptableObject
{
    [HideInInspector]
    public UnityEvent<GameObject> DrinkShowStart;
    [HideInInspector]
    public UnityEvent DrinkShowEnd;
}

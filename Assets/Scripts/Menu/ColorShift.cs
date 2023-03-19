using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorShift : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField][Range(0f, 1f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0; 

    float t = 0f;

    int lenght;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lenght = myColors.Length;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= lenght) ? 0 : colorIndex;
        }
    }
}

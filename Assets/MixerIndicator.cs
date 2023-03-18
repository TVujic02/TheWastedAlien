using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerIndicator : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The distance between each sprite.")]
    private float distanceBetweenSprites = 1f;

    [SerializeField]
    [Tooltip("Refrence to the sprites parent object.")]
    private GameObject spriteParent;

    [SerializeField]
    [Tooltip("Reference to the scrap button.")]
    private GameObject scrapButton;

    [SerializeField]
    [Tooltip("The indidcator sprite prefab.")]
    private GameObject indidcatorSpritePrefab;

    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    private float totalLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateIndicator(List<Ingridient> ingridientList)
    {
        //Clear the previous indicator
        foreach(SpriteRenderer renderer in renderers) 
        {
            Destroy(renderer.gameObject);
        }
        renderers.Clear();

        totalLength = ingridientList.Count * distanceBetweenSprites; //Get the total length
        float startPosX = transform.position.x - totalLength / 2; //Use the total length to get the startpos

        //Create the new layout
        for(int i = 0; i < ingridientList.Count; i++)
        {
            GameObject obj = Instantiate(indidcatorSpritePrefab, new Vector3(startPosX + (i * distanceBetweenSprites), transform.position.y), Quaternion.identity, spriteParent.transform); //Spawn at correct position
            SpriteRenderer indicatorRenderer = obj.GetComponent<SpriteRenderer>(); 
            indicatorRenderer.sprite = ingridientList[i].GetIngridientSprite; //Get the sprite
            renderers.Add(indicatorRenderer);
        }
        scrapButton.transform.position = new Vector3(transform.position.x + totalLength/2, transform.position.y); //Set the position of the scrap button
        if (ingridientList.Count == 0)
            scrapButton.SetActive(false);
        else
            scrapButton.SetActive(true);
        
    }
}

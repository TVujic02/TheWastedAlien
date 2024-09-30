using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    bool dragging;
    IMouseInteractable currentInteractable = null;

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            if (!dragging)
            {
                Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(cameraRay);

                //Get the first interactable that was clicked on
                IMouseInteractable interactable = hits.AsEnumerable().Select(i => i.collider.gameObject.GetComponentInParent<IMouseInteractable>())
                    .Where(i => i != null).FirstOrDefault();

                //Null check
                if(interactable != null)
                {
                    interactable.MouseDown();
                    currentInteractable = interactable;

                    //Start dragging
                    dragging = true;
                }
            }
            else
            {
                //If we are draggind and the mouse button is still pressed we keep on dragging
                currentInteractable.MouseDrag();
            }
        }
        else if(dragging == true)
        { 
            //If we just released an interactable we call MouseUp
            currentInteractable.MouseUp();
            dragging = false;
        }
    }
}

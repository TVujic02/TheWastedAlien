using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseInteractable
{
    public abstract void MouseDown();

    public abstract void MouseDrag();

    public abstract void MouseUp();

}

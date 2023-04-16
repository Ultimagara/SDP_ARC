using System;
using UnityEngine;
using UnityEngine.UI;

public class RemoteToggle : MonoBehaviour
{
    public CanvasGroup ToggleAlpha;
    public float fadeTo;

    // Start is called before the first frame update
    public void Toggle()
    {
        if (ToggleAlpha.alpha != 1F)
            ToggleAlpha.alpha = 1F;
        else
            ToggleAlpha.alpha = fadeTo;
    }

}

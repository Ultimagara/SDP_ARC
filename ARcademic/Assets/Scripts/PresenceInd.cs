using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PresenceInd : MonoBehaviour
{
    public CanvasGroup alphaTarget;
    public string targetFilename;
    public float defaultAlpha;
    public float interval;

    void Start()
    {
        InvokeRepeating("CheckPresent", 0, interval);
    }

    void CheckPresent()
    {
        if (File.Exists(Application.persistentDataPath + "/" + targetFilename))
            alphaTarget.alpha = 1F;
        else
            alphaTarget.alpha = defaultAlpha;
    }

}

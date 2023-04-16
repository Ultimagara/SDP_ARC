using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewSession : MonoBehaviour
{
    public void InitializeSession()
    {
        DateTime mom = DateTime.Now;
        string path = Application.persistentDataPath + string.Format("/transpose_{0}-{1}-{2}.txt", mom.Month, mom.Day, mom.Year);
    }
}

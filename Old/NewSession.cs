using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NewSession : MonoBehaviour
{
    public void InitializeSession()
    {
        // Define destination
        DateTime mom = DateTime.Now;
        string newPath = Application.persistentDataPath + string.Format(
            "/archived_{0}{1}{2}_{3}{4}.txt", mom.Month, mom.Day, mom.Year, mom.Hour, mom.Minute);

        // Get existing
        List<string> listPaths = new List<string>();
        string f1 = Application.persistentDataPath + "/speechTranspose.txt";
        string f2 = Application.persistentDataPath + "/recognizedText.txt";
        if (File.Exists(f1)) listPaths.Add(f1);
        if (File.Exists(f2)) listPaths.Add(f2);
        
        // Shove existing data into new file for archiving, delete
        using (var outputStream = File.OpenWrite(newPath)) {
            foreach (var path in listPaths) {
                using (var inputStream = File.OpenRead(path)) {
                    inputStream.CopyTo(outputStream);
                }
                File.Delete(path);
            }
        }
    }
}

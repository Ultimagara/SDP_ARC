using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Globalization;

public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    public Text ResultText;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        Debug.Log(obj);
        ResultText.text = "Recognized: ";
        var result = new RecognitionResult(obj);
        ResultText.text += result.Phrases[0].Text + "\n" + "Confidence: " + result.Phrases[0].Confidence;

        // Write output to file
        DateTime mom = DateTime.Now;
        string path = Application.persistentDataPath + "/speechTranspose.txt";
        DateTime lwt = Directory.GetLastWriteTime(path);

        if ((mom.Hour != lwt.Hour) | (mom.Minute != lwt.Minute) | (mom.Second != lwt.Second))
        {
            Debug.Log("Writing to: " + path);
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(result.Phrases[0].Text);
            writer.Close();
        }
    }

}

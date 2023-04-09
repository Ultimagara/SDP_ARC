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
        string path = Application.persistentDataPath + string.Format("/transpose_{0}-{1}-{2}.txt", mom.Month, mom.Day, mom.Year);
        DateTime lwt = Directory.GetLastWriteTime(path);

        if ((mom.Hour != lwt.Hour) | (mom.Minute != lwt.Minute) | (mom.Second != lwt.Second))
        {
            Debug.Log("Writing to: " + path);
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(result.Phrases[0].Text);
            writer.Close();
        }

        /*
        for (int i = 0; i < result.Phrases.Length; i++) {
            if (i > 0)
                ResultText.text += "\n ---------- \n";
            ResultText.text += result.Phrases[0].Text + " | " + "Confidence: " + result.Phrases[0].Confidence;
        }
        */
    }
}

using UnityEngine;
using System.IO;
public class FileIO : MonoBehaviour
{
    public static void WriteString(string toWrite, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        Debug.Log("Writing to: " + path);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(toWrite);
        writer.Close();;
    }

    public static string ReadString(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        Debug.Log("Reading from: " + path);
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string toReturn = reader.ReadToEnd();
        reader.Close();
        return toReturn;
    }
}

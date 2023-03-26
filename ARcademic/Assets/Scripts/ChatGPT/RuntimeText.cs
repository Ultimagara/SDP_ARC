using UnityEngine;
using System.IO;
public class RuntimeText : MonoBehaviour
{
    public static void WriteString(string toWrite, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName;
        Debug.Log("Writing to: " + path);

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(toWrite);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
    public static string ReadString()
    {
        string path = Application.persistentDataPath + "/test.txt";
        Debug.Log("Reading from: " + path);

        //Read the text directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string toReturn = reader.ReadToEnd();
        reader.Close();
        return toReturn;
    }
}

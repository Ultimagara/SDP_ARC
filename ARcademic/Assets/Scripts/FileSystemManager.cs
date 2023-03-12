using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class FileSystemManager
{ 
    string MainPath = Path.Combine(Application.persistentDataPath, "/FileSystem");

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void FileWritePicture(string Pathname, Texture2D Picture)
    {

    }
    void FileWriteText(string Pathname, string AudioTranslation)
    {
        FileStream filestream = new FileStream(MainPath, FileMode.Create);
        Byte[] info = new UTF8Encoding(true).GetBytes(AudioTranslation);
        filestream.Write(info);
        filestream.Close();


    }



}

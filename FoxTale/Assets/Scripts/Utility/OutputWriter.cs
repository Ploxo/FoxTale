using UnityEngine;
using System.IO;

public class OutputWriter : MonoBehaviour
{
    public static void WriteString(string filePath, string text, bool append)
    {
        string path = Application.persistentDataPath + "/" + filePath;

        //Write some text to the file
        StreamWriter writer = new StreamWriter(path, append);

        writer.WriteLine(text);
        writer.Close();

        StreamReader reader = new StreamReader(path);

        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        Debug.Log($"Output to path: '{Application.persistentDataPath}/{filePath}', string:\n{text}");

        reader.Close();
    }

    public static void ReadString(string filePath)
    {
        string path = Application.persistentDataPath + "/" + filePath;

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        Debug.Log(reader.ReadToEnd());

        reader.Close();
    }
}

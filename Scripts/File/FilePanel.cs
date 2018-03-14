

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class FilePanel : EditorWindow
{
    /// <summary>
    /// Save data into file
    /// </summary>
    /// <param name="data"></param>
    public static void Save(string data, string defaultName = "Unnamed", string extension = "json")
    {

        string path = EditorUtility.SaveFilePanelInProject("Save file", defaultName, extension, "json",Application.dataPath);
        if (path.Length != 0)
        {
            File.WriteAllText(path, data, new UTF8Encoding(false));//UTF8 WITHOUT BOM, BOM WILL CAUSE AN INVISIBLE ERROR IN UNITY
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Load data from file
    /// </summary>
    /// <returns></returns>
    public static string Load()
    {
        string data = "";
        string path = EditorUtility.OpenFilePanel("Load file", Application.dataPath, "json");
        if (path.Length != 0)
        {
            data = File.ReadAllText(path);
        }

        return data;
    }

    protected static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    protected static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
}
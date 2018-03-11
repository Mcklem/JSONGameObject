using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;

public class JSONSceneManager : EditorWindow
{
    static string GeneratedJSONPath
    {
        get
        {
            return Application.dataPath + "/JSONGameObject/Editor/GeneratedJSON/";
        }
    }

    static string CurrentTime
    {
        get
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }

    [MenuItem("JSONTool/JSON From Scene")]
    static void Init()
    {
        string path = GeneratedJSONPath + CurrentTime + ".json";

        //File.Create(path);
        File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(GetAllJSONObjectsInScene()));
        //File.WriteAllBytes(path, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(GetAllJSONObjectsInScene())));
    }

    public static List<JSONGameObject> GetAllJSONObjectsInScene()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        List<JSONGameObject> allJSONGameObjects = new List<JSONGameObject>();
        foreach (GameObject go in allGameObjects)
        {
            allJSONGameObjects.Add(go.ToJSONGameObject());
        }
        return allJSONGameObjects;
    }
}

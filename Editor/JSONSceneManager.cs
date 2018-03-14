using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;

public class JSONSceneManager : EditorWindow
{


    static string CurrentTime
    {
        get
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }

    [MenuItem("JSONGameObject/JSON From Scene GameObjects")]
    static void JSONFromSceneGameObjects()
    {
        FilePanel.Save(Newtonsoft.Json.JsonConvert.SerializeObject(GetAllJSONObjectsInScene()), CurrentTime);
    }

    [MenuItem("JSONGameObject/Scene GameObjects From JSON")]
    static void SceneGameObjectsFromJSON()
    {
        List<JSONGameObject> jsonGameObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<JSONGameObject>>(FilePanel.Load());
        jsonGameObjects.ToGameObjects();
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

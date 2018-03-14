using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;

public class JSONGameObjectTesting : MonoBehaviour {

    /// <summary>
    /// Path of the JSONGameObject to test
    /// </summary>
    public TextAsset jsonFile;
    public GameObject gameObjectToSerialize;

    private void OnGUI()
    {

        JSONGameObjectManager.DebugEnabled = GUILayout.Toggle(JSONGameObjectManager.DebugEnabled, "JSON Debug Enabled?");
        if (GUILayout.Button("Instantiate from a gameObject copy"))
        {
            if (gameObjectToSerialize)
            {
                //Test it, Serialize and deserialize the object to make sure all properties are okay

                //Convert a GameObject into a json serializable object "JSONGameObject"
                JSONGameObject jsonGameObject = gameObjectToSerialize.ToJSONGameObject();

                //Print the result
                Debug.Log(JsonConvert.SerializeObject(jsonGameObject));

                //Create a new gameObject from the jsonGameObject data
                jsonGameObject.ToGameObject();
            }
            else
            {
                Debug.LogError("No gameobject to serialize");
            }
        }

        if (GUILayout.Button("Instantiate gameObject from JSON File"))
        {
            JSONGameObject jsonGameObject = JsonConvert.DeserializeObject<JSONGameObject>(jsonFile.text);
            jsonGameObject.ToGameObject();
        }

        if (GUILayout.Button("Instantiate a list of gameObjects from JSON File"))
        {
            List<JSONGameObject> jsonGameObjects = JsonConvert.DeserializeObject<List<JSONGameObject>>(jsonFile.text);
            jsonGameObjects.ToGameObjects();
        }
        if (GUILayout.Button("Testing custom deserialization"))
        {
            JSONGameObject jsonGameObjects = JSONGameObjectManager.Serialize(jsonFile.text);
            jsonGameObjects.ToGameObject();
        }
    }

}

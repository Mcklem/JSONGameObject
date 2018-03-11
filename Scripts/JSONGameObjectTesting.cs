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
    public string jsonPath;
    public GameObject gameObjectToSerialize;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(jsonPath);
            JSONGameObject jsonGameObject = JsonConvert.DeserializeObject<JSONGameObject>(textAsset.text);
            jsonGameObject.ToGameObject();
        }
    }

}

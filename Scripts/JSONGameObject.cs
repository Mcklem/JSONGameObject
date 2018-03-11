using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

[Serializable]
public class JSONGameObject {

    //Coming soon
    //public List<JSONProperty> properties = new List<JSONProperty>();

    public string name;
    public List<JSONComponent> components = new List<JSONComponent>();

    public JSONGameObject()
    {

    }

    

    public JSONGameObject(string name, List<JSONComponent> components)
    {
        this.name = name;
        this.components = components;
    }

    public static Type GetTypeByName(string name)
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.Name == name)
                    return type;
            }
        }

        return null;
    }
    
}

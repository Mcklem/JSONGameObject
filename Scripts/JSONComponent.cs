using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[Serializable]
public class JSONComponent
{
    public string name;
    public List<JSONProperty> properties;

    public JSONComponent()
    {
    }

    public JSONComponent(string name)
    {
        this.name = name;
    }

    public JSONComponent(string name, List<JSONProperty> properties) : this(name)
    {
        this.properties = properties;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JSONProperty {

    public string name;
    public object value;

    public JSONProperty()
    {
    }

    public JSONProperty(string name, object value)
    {
        this.name = name;
        this.value = value;
    }
}

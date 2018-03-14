using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class JSONProperty {

    public string name;
    public string type;
    public object value;

    public JSONProperty()
    {
    }

    public JSONProperty(string name, string type,object value)
    {
        this.name = name;
        this.type = type;
        this.value = value;
    }

    public override string ToString()
    {
        return string.Format("Name {0} Type {1} Value {2}",name, type, value);
    }
}

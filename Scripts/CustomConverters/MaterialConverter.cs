using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class MaterialConverter : JsonCreationConverter<Material>
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Debug.Log("Mesh to json: " + value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

        return base.ReadJson(reader, objectType, existingValue, serializer);
    }
    protected override Material Create(Type objectType, JObject jObject)
    {
        try
        {
            Material material = new Material(Shader.Find(" Diffuse"));
            
            return material;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error deserializing material " + jObject.ToString());
            return null;
        }
    }
}

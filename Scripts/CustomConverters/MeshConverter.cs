using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class MeshConverter : JsonCreationConverter<Mesh>
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Debug.Log("Mesh to json: " + value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

        return base.ReadJson(reader, objectType, existingValue, serializer);
    }
    protected override Mesh Create(Type objectType, JObject jObject)
    {
        try
        {
            Mesh mesh = new Mesh();
            return mesh;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error deserializing " + jObject["_id"].ToString());
            return null;
        }
    }
}

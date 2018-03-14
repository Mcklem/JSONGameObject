using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class JSONGameObjectManager
{
    static bool debug = false;

    public static bool DebugEnabled
    {
        get
        {
            return debug;
        }
        set
        {
            debug = value;
        }
    }

    /// <summary>
    /// Construct JSONGameObject from a JSON
    /// </summary>
    /// <param name="gameObject"></param>
    public static JSONGameObject Serialize(string json)
    {
        JSONGameObject jsonGameObject = null;
        try
        {
            jsonGameObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONGameObject>(json);
            foreach (JSONComponent jsonComponent in jsonGameObject.components)
            {
                foreach (JSONProperty jsonProperty in jsonComponent.properties)
                {
                    if (jsonProperty.value == null) continue;

                    Debug.Log(jsonProperty);
                    Type type = GetTypeByName(jsonProperty.type);

                    if (type.IsEnum)//Enums
                    {
                        if (debug) Debug.Log("ENUM[" + type.Name + "]");
                        jsonProperty.value = Enum.Parse(type, jsonProperty.value.ToString());
                    }
                    else if (type.IsPrimitive)//Primitive values
                    {
                        if (debug) Debug.Log("PRIMITIVE[" + type.Name + "]");
                        //propertyInfo.SetValue(unityComponent, Convert.ChangeType(property.value, propertyInfo.PropertyType), null);
                        jsonProperty.value = Convert.ChangeType(jsonProperty.value, type);
                    }
                    else if (type.IsValueType)//Structs
                    {
                        if (debug) Debug.Log("STRUCT[" + type.Name + "]");
                        //propertyInfo.SetValue(unityComponent, property.value, null);
                        jsonProperty.value = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonProperty.value.ToString(), type);
                    }
                    else
                    {
                        if (debug) Debug.Log("OTHER[" + type.Name + "]");
                        jsonProperty.value = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonProperty.value.ToString(), type);
                    }

                    Debug.Log(jsonProperty);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        return jsonGameObject;
    }

    /// <summary>
    /// Construct JSONGameObject from a gameObject
    /// </summary>
    /// <param name="gameObject"></param>
    static JSONGameObject JSONGameObject(GameObject gameObject)
    {
        JSONGameObject jsonGameObject = new JSONGameObject();
        jsonGameObject.name = gameObject.name;
        jsonGameObject.components = new List<JSONComponent>();
        try
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                List<JSONProperty> jproperty = new List<JSONProperty>();
                JSONComponent jComponent = new JSONComponent(component.GetType().Name, jproperty);
                PropertyInfo[] properties = component.GetType().GetProperties();

                //Avoid component properties    
                List<string> avoidablePropertyNames = GetAvoidablePropertyNames();

                foreach (PropertyInfo property in properties)
                {
                    //Avoid  properties
                    if (avoidablePropertyNames.Contains(property.Name))
                    {
                        if(debug) Debug.LogWarning("Skipped set value for property " + property.Name + " for component " + component.GetType().Name + " for object " + gameObject.name);
                        continue;
                    }

                    if (property.GetSetMethod() != null)
                    {
                        jproperty.Add(new JSONProperty(property.Name, property.PropertyType.Name, component.GetType().GetProperty(property.Name).GetValue(component, null)));
                    }
                    else
                    {
                        if (debug) Debug.LogWarning("property " + property.Name + " of component " + component.GetType().Name + " of object " + gameObject.name + " doesnt have SetMethod");
                    }
                }

                jsonGameObject.components.Add(jComponent);
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return jsonGameObject;
    }

    /// <summary>
    /// Create a GameObject instance from the current JSONGameObject
    /// </summary>
    /// <returns></returns>
    static GameObject GameObject(JSONGameObject jSONGameObject)
    {
        //Create gameObject
        GameObject gameObject = new GameObject(jSONGameObject.name);

        foreach (JSONComponent jComponent in jSONGameObject.components)
        {
            //Getting tipe from this way is slow, it can be optimized
            Type type = GetTypeByName(jComponent.name);//Type.GetType(jComponent.name);
            if (type == null)
            {
                if (debug) Debug.LogError("Type doesnt exists: [" + jComponent.name + "]");
                continue;
            }

            //Load components
            Component unityComponent = null;
            if (typeof(Component).IsAssignableFrom(type))
            {
                unityComponent = gameObject.GetComponent(type);
                if (unityComponent)
                {
                    if (debug) Debug.LogWarning(jComponent.name + " is already added to " + jSONGameObject.name);
                }
                else
                {
                    unityComponent = gameObject.AddComponent(type);
                }
            }
            else
            {
                if (debug) Debug.LogError("Class " + jComponent.name + " is not a class derived from Component and cannot be added as a component of GameObject: " + jComponent.name);
                continue;
            }

            //Set properties
            foreach (JSONProperty property in jComponent.properties)
            {
                PropertyInfo propertyInfo = unityComponent.GetType().GetProperty(property.name);
                if (propertyInfo == null)
                {
                    if (debug) Debug.LogError("Propery [" + property.name + "] of component " + jComponent.name + " doesnt exists.");
                    continue;
                }

                //Not writable property
                if (!propertyInfo.CanWrite || propertyInfo.GetSetMethod() == null)
                {
                    if (debug) Debug.LogError("Propery [" + property.name + "] of component " + jComponent.name + " cant be written or doesnt have set method.");
                    continue;
                }

                //Null avoidance
                if (property.value == null)
                {
                    if (debug) Debug.LogWarning("Propery [" + property.name + "] of component " + jComponent.name + " is null and wont be set");
                    continue;
                }

                try
                {
                    if (debug) Debug.Log(propertyInfo.Name + " " + property.value.ToString() + " " + propertyInfo.PropertyType.ToString());

                    if (propertyInfo.PropertyType.IsEnum)//Enums
                    {
                        if (debug) Debug.Log("ENUM[" + propertyInfo.PropertyType.Name + "]");
                        propertyInfo.SetValue(unityComponent, property.value, null);
                    }
                    else if (propertyInfo.PropertyType.IsPrimitive)//Primitive values
                    {
                        if (debug) Debug.Log("PRIMITIVE[" + propertyInfo.PropertyType.Name + "]");
                        propertyInfo.SetValue(unityComponent, Convert.ChangeType(property.value, propertyInfo.PropertyType), null);
                    }
                    else if (propertyInfo.PropertyType.IsValueType)//Structs
                    {
                        if (debug) Debug.Log("STRUCT[" + propertyInfo.PropertyType.Name + "]");
                        propertyInfo.SetValue(unityComponent, property.value, null);
                    }
                    else
                    {
                        if (debug) Debug.Log("OTHER[" + propertyInfo.PropertyType.Name + "]");
                        if (debug) Debug.LogWarning(" property " + propertyInfo.Name + " of component " + jComponent.name + " of object " + jSONGameObject.name + " " + property.value.ToString() + " is not a valueType");
                        /*
                        if (property.value != null && property.value.ToString() != "None")
                        {
                            JObject o = JObject.Parse(property.value.ToString());//Parsed  json property as jobject
                            foreach (FieldInfo field in propertyInfo.PropertyType.GetFields()) //FieldInfo prop in 
                            {
                                var value = o.GetValue(field.Name);
                                if (debug) Debug.Log(field.Name);
                                field.SetValue(field, value);
                            }
                        }*/
                        try
                        {
                            propertyInfo.SetValue(unityComponent, property.value, null);
                        }
                        catch (ArgumentException e)
                        {
                            propertyInfo.SetValue(unityComponent, Newtonsoft.Json.JsonConvert.DeserializeObject(property.value.ToString()), null);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }

                }
                catch (Exception e)
                {
                    //e.StackTrace + " " + 
                    if (debug) Debug.LogError(e.GetType().Name + " " + e.Message + " for property " + property.name + " for component " + jComponent.name + " for object " + jSONGameObject.name);
                }

            }

        }

        return gameObject;
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

    /// <summary>
    /// Get a list of restringed property names
    /// </summary>
    /// <returns></returns>
    private static List<string> GetAvoidablePropertyNames()
    {
        List<string> avoidablePropertyNames = new List<string>();
        foreach (PropertyInfo avoidableProperty in typeof(Component).GetProperties())
        {
            avoidablePropertyNames.Add(avoidableProperty.Name);
        }
        return avoidablePropertyNames;
    }

    #region extensions

    /// <summary>
    /// Construct JSONGameObject from a gameObject values
    /// </summary>
    /// <param name="gameObject"></param>
    public static JSONGameObject ToJSONGameObject(this GameObject gameObject)
    {
        return JSONGameObject(gameObject);
    }

    /// <summary>
    /// Create a new gameobject from JSONGameObject data
    /// </summary>
    /// <param name="JSONGameObject"></param>
    /// <returns></returns>
    public static GameObject ToGameObject(this JSONGameObject JSONGameObject)
    {
        return GameObject(JSONGameObject);
    }

    /// <summary>
    /// Create a new gameobject from JSONGameObject data
    /// </summary>
    /// <param name="JSONGameObjects"></param>
    /// <returns></returns>
    public static List<GameObject> ToGameObjects(this List<JSONGameObject> JSONGameObjects)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach (JSONGameObject JSONGameObject in JSONGameObjects)
        {
            gameObjects.Add(JSONGameObject.ToGameObject());
        }
        return gameObjects;
    }
    #endregion

    #region NOT USED
    //Avoid iterations when a assembly dictionary is created
    static Dictionary<string, Type> assemblyTypes = new Dictionary<string, Type>();
    static Type GetTypeFromName(string name)
    {
        if (assemblyTypes == null || assemblyTypes.Keys.Count <= 0)
        {
            InitializeAssemblyTypes();
        }

        if (assemblyTypes.ContainsKey(name)) return assemblyTypes[name];
        else return null;
    }

    static void InitializeAssemblyTypes()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            //ADD ONLY UNITY AND MONO ASSEMBLIES
            if (assembly.FullName.Contains("Assembly") || assembly.FullName.Contains("UnityEngine"))
            {
                foreach (Type type in assembly.GetTypes())
                {
                    //ADD ONLY COMPONENT TYPES
                    if (typeof(Component).IsAssignableFrom(type))
                    {
                        //Debug.Log(assembly.FullName + " - " + type.Name + " - " + type.FullName + " - " + type.AssemblyQualifiedName);
                        if (!assemblyTypes.ContainsKey(type.Name)) assemblyTypes.Add(type.Name, type);
                        //else Debug.LogError("Error at " + type.Name);
                    }

                }
            }
        }
    }

    /// <summary>
    /// Construct new vector from string values
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static Vector3 StringToVector3(string data)
    {
        string[] temp = data.Substring(1, data.Length - 2).Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }
    #endregion

}

# JSONGameObject
JSONGameObject allows to serialize/deserialize Unity3D GameObjects from JSON format.


## Getting Started

Download and insert under /Assets folder of your Unity3D project, go to "What is required?" and no more is required.

### What is required?

Requires Newtonsoft library or another deep JSON serializer, (JsonUtility from Unity3D doesnt work) 
https://github.com/SaladLab/Json.Net.Unity3D
https://github.com/SaladLab/Json.Net.Unity3D/releases

### Built with extension methods

Make it simple, use a gameObject, print or store the JSON result.

```
JSONGameObject jsonGameObject = gameObjectToSerialize.ToJSONGameObject();

//Print the result (Example made with NEWTONSOFT serialization)[NEWTONSOFT](https://github.com/SaladLab/Json.Net.Unity3D/releases)
Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(jsonGameObject));

//Create a new gameObject from the jsonGameObject data
GameObject newGameObject = jsonGameObject.ToGameObject();
```

You can also load the json from WWW service or from file

```
//Get a textAsset (JSON generated or modified)
TextAsset textAsset = Resources.Load<TextAsset>(jsonPath);

//Use NEWTONSOFT or another json library to deserialize the string into a "JSONGameObject"
JSONGameObject jsonGameObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONGameObject>(textAsset.text);

//Create a new gameObject from the generated jsonGameObject
jsonGameObject.ToGameObject();
```

## Authors

* **Jesus García Urtiaga** - *Initial work*

See also the list of [contributors](https://github.com/Mcklem/JSONGameObject/graphs/contributors) who participated in this project.

## License

This project is licensed under the WTFPL – Do What the Fuck You Want to Public License - see the [WTFPL](http://www.wtfpl.net/) web page for details.

## Acknowledgments

* All code is c#
* Based in System.Reflections to generate/fill GameObjects and Components variables
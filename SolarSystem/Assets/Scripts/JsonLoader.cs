using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonLoader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class CelestialJson
    {
        public string name;
        public float size;
        public float distance;
        public float mass;
    }

    [System.Serializable]
    public class CelestialList
    {
        public CelestialJson[] celestialJson;
    }

    public CelestialList myCelestialList = new CelestialList();

    void loadJson()
    {
                myCelestialList = JsonUtility.FromJson<CelestialList>(textJSON.text);
        Debug.Log(myCelestialList.celestialJson[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        //textJSON = Resources.Load<TextAsset>("JSONText");
        myCelestialList = JsonUtility.FromJson<CelestialList>(textJSON.text);
        Debug.Log(myCelestialList.celestialJson[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

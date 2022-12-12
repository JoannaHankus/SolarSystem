using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CelestialController : MonoBehaviour
{

    static CelestialController instance;
    Celestial[] celestials;
    float sun_distance = 0;

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



    void Start()
    {
        

        textJSON = Resources.Load<TextAsset>("JSONText");
        
        myCelestialList = JsonUtility.FromJson<CelestialList>(textJSON.text);
        Debug.Log(myCelestialList.celestialJson[0].name);
        var texture_skybox = Resources.Load<Texture2D>("Textures/skybox");
        var cam = FindObjectOfType<Camera>();
        cam.gameObject.AddComponent<CameraMovement>();
        Material skyboxMaterial = Resources.Load<Material>("Materials/MilkyWay");
        RenderSettings.skybox = skyboxMaterial;

        Time.fixedDeltaTime = UniverseData.physicsTimeStep;
        for (int i =0; i< 9; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sun_distance = myCelestialList.celestialJson[i].distance * 10;
            var celestial_diameter = 1f;

            sphere.transform.position = new Vector3(0, 0, sun_distance);
            sphere.AddComponent<SphereCollider>();
            sphere.AddComponent<TrailRenderer>();
            sphere.GetComponent<TrailRenderer>().time = 180;
            sphere.GetComponent<TrailRenderer>().widthMultiplier = 15;
            sphere.AddComponent<Rigidbody>();
            sphere.AddComponent<Celestial>();
            sphere.GetComponent<Celestial>().rb = sphere.GetComponent<Rigidbody>();
            sphere.GetComponent<Celestial>().rb.useGravity = false;
            sphere.GetComponent<Celestial>().rb.mass = myCelestialList.celestialJson[i].mass;

            sphere.GetComponent<Celestial>().bodyName = myCelestialList.celestialJson[i].name;

            if (sphere.GetComponent<Celestial>().bodyName == "Sun") {
                sphere.GetComponent<Celestial>().radius = myCelestialList.celestialJson[i].size;
                Debug.Log(sphere.GetComponent<Celestial>().radius);
            }
            else { 
                sphere.GetComponent<Celestial>().radius = myCelestialList.celestialJson[i].size * 100;
                Debug.Log(sphere.GetComponent<Celestial>().radius);

            }
            sphere.name = myCelestialList.celestialJson[i].name;
            if (sphere.GetComponent<Celestial>().bodyName == "Sun")
            {
                celestial_diameter = myCelestialList.celestialJson[i].size;
            }
            else
            {
                celestial_diameter = myCelestialList.celestialJson[i].size * 10;

            }
            sphere.transform.localScale = new Vector3(celestial_diameter, celestial_diameter, celestial_diameter);

            Material newMaterial = new Material(Shader.Find("Standard"));

            var texture = Resources.Load<Texture2D>("Textures/" + myCelestialList.celestialJson[i].name);
            newMaterial.SetTexture("_MainTex", texture);
            sphere.GetComponent<MeshRenderer>().material = newMaterial;

        }
        celestials = FindObjectsOfType<Celestial>();
        foreach (Celestial a in celestials)
        {
            if(a.bodyName == "Sun")
            {
                
                cam.transform.position = new Vector3(0, 5000, 0);
                cam.transform.LookAt(a.transform);
            }
        }
        InitialVelocity();

        
    }


    void FixedUpdate()
    {
        Gravity();
    }

    void Gravity()
    {
        Vector3 acceleration = Vector3.zero;

        foreach (Celestial a in celestials)
        {
            foreach(Celestial b in celestials)
            {
                if (!a.Equals(b))
                {
                    float m1 = a.rb.mass;
                    float m2 = b.rb.mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    Vector3 forceDir = (b.transform.position - a.transform.position).normalized;
                    a.GetComponent<Rigidbody>().AddForce(forceDir * (UniverseData.gravitationalConstant * (m1 * m2) / (r * r)));

                }
            }
            a.UpdateVelocity(acceleration, UniverseData.physicsTimeStep);
            a.UpdatePosition(UniverseData.physicsTimeStep);
        }
    }

    void InitialVelocity()
    {
        foreach (Celestial a in celestials)
        {
            foreach (Celestial b in celestials)
            {
                if (!a.Equals(b))
                {
                    float m2 = b.rb.mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);
                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((UniverseData.gravitationalConstant * m2) / r);
                }
            }
        }

    }



    public static Vector3 CalculateAcceleration(Vector3 point, Celestial ignoreBody = null)
    {
        Debug.Log("Point: " + point.ToString());


        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.celestials)
        {
            Debug.Log("Body: " + body.bodyName);
            if (body != ignoreBody)
            {
                float sqrDst = (body.rb.position - point).sqrMagnitude;
                Debug.Log("sqrDstr: " + sqrDst);

                Vector3 forceDir = (body.rb.position - point).normalized;
                Debug.Log("ForceDir: " + forceDir);

                acceleration += forceDir * UniverseData.gravitationalConstant * body.mass / sqrDst;

            }
            Debug.Log(body.name + " mass: " + body.mass);
            
        }
        return acceleration;
    }

    static CelestialController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CelestialController>();
            }
            return instance;
        }
    }
}

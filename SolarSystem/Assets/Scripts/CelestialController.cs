using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CelestialController : MonoBehaviour
{

    static CelestialController instance;
    Celestial[] celestials;
    public List<string> celestial_names = new List<string>() { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };

    private List<float> celestial_sizes = new List<float>() { 696f, 2.4f, 6f, 6.3f, 3.4f, 15f, 12f, 5f, 4f };
    private List<float> celestial_distances = new List<float>() { 0, 50f, 100f, 151.48f, 200f, 300f, 380f, 460f, 540f };

    private List<float> celestial_masses = new List<float>() { 333000f, 0.055f, 0.815f, 1, 0.107f, 318f, 95, 14, 17 };

    float sun_distance = 0;

    //public TextAsset textJSON;

    //[System.Serializable]
    //public class CelestialJson
    //{
    //    public string name;
    //    public float size;
    //    public float distance;
    //    public float mass;
    //}

    //[System.Serializable]
    //public class CelestialList
    //{
    //    public CelestialJson[] celestialJson;
    //}

    //public CelestialList myCelestialList = new CelestialList();

   

    void Start()
    {
        //textJSON = Resources.Load<TextAsset>("JSONText");
        //myCelestialList = JsonUtility.FromJson<CelestialList>(textJSON.text);
        //Debug.Log(myCelestialList.celestialJson[0]);
        //JsonLoader.loadJson();
        var texture_skybox = Resources.Load<Texture2D>("Textures/skybox");
        var cam = FindObjectOfType<Camera>();
        cam.gameObject.AddComponent<CameraMovement>();
        Material skyboxMaterial = Resources.Load<Material>("Materials/MilkyWay");
        RenderSettings.skybox = skyboxMaterial;

        Time.fixedDeltaTime = UniverseData.physicsTimeStep;
        for (int i =0; i< 9; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sun_distance = celestial_distances[i] * 10;
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
            sphere.GetComponent<Celestial>().rb.mass = celestial_masses[i];

            sphere.GetComponent<Celestial>().bodyName = celestial_names[i];

            if (sphere.GetComponent<Celestial>().bodyName == "Sun") {
                sphere.GetComponent<Celestial>().radius = celestial_sizes[i];
                Debug.Log(sphere.GetComponent<Celestial>().radius);
            }
            else { 
                sphere.GetComponent<Celestial>().radius = celestial_sizes[i] * 100;
                Debug.Log(sphere.GetComponent<Celestial>().radius);

            }
            sphere.name = celestial_names[i];
            if (sphere.GetComponent<Celestial>().bodyName == "Sun")
            {
                celestial_diameter = celestial_sizes[i] ;
            }
            else
            {
                celestial_diameter = celestial_sizes[i] * 10;

            }
            sphere.transform.localScale = new Vector3(celestial_diameter, celestial_diameter, celestial_diameter);

            Material newMaterial = new Material(Shader.Find("Standard"));

            var texture = Resources.Load<Texture2D>("Textures/" + celestial_names[i]);
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
                    //acceleration += forceDir * (UniverseData.gravitationalConstant * (m1 * m2)/ (r * r));
                    //Debug.Log(a.bodyName + " " + b.bodyName);
                    //a.UpdateVelocity(acceleration, UniverseData.physicsTimeStep);
                    //a.UpdatePosition(UniverseData.physicsTimeStep);



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
                    //a.velocity += a.transform.right * Mathf.Sqrt((UniverseData.gravitationalConstant * m2) / r);
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

                //wypadkowa przyspieszen
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

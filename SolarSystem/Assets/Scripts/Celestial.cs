using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celestial : MonoBehaviour
{
    public Vector3 initialVelocity;

    public string bodyName = "Unnamed";
    public float radius;
    public Vector3 velocity;
    public float mass;
    public float scaleFactor = 1;

    public Rigidbody rb;

    private void Awake()
    {
        velocity = initialVelocity;
        scaleFactor = 1;
    }


    void OnValidate()
    {
        velocity = initialVelocity;
    }


    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + velocity * timeStep);

    }

    public void ScaleCelestial(float scale)
    {
        Debug.Log(bodyName + " " + scale.ToString());
        rb.mass = rb.mass / scaleFactor * scale;
        Debug.Log(mass.ToString() + " " + rb.mass.ToString());

        transform.localScale = transform.localScale / scaleFactor * scale;
        scaleFactor = scale;
        
    }
}

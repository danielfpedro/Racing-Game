using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

    public float drag = 1f;
    public Rigidbody rb;
    public float turnSpeed;
    public float grip;

    public SimpleGearBoxController gearBox;

    public Transform centerOfMass;
    public float burnoutIntensity;
    public AudioSource engineSound;

    // Use this for initialization
    void Start () {
	}

    void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float desiredTurnSpeed = turnSpeed;
        if (Input.GetButton("Fire2"))
        {
            desiredTurnSpeed = turnSpeed * 2f;
        }
        transform.Rotate(Vector3.up, Time.deltaTime * desiredTurnSpeed * Input.GetAxis("Horizontal"), Space.World);

        if (rb.velocity.magnitude >= .01f)
        {
            // rb.AddRelativeForce(Vector3.back * (GetSpeed() / 100f), ForceMode.Acceleration);
        }
        // rb.AddRelativeForce(Vector3.back * (GetSpeed() / 30f), ForceMode.Acceleration);


        float grip = 1000f;
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        rb.AddForce(transform.right * -GetVelocityX() * grip);
    }

    public float GetSpeed()
    {
        return Mathf.Clamp(rb.velocity.magnitude * 3.6f, 0f, 10000f);
    }

    public float GetVelocityX()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        return velocity.x;
    }
}

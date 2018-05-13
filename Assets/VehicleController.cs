using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

    public float drag = 1f;
    public Rigidbody rb;
    public float turnSpeed;
    public float grip;

    public SimpleGearBoxController gearBox;

    public float burnoutIntensity;
    public AudioSource engineSound;

    public bool grounded = false;
    public bool onAir = false;
    public LayerMask whatIsGround;

    public Transform groundCheckers;

    private float desiredTurnSpeed;

    public GameObject wheels;

    private float lastSteering;
    // Use this for initialization
    void Start () {
        // rb.centerOfMass = cof.localPosition;

        lastSteering = 0f;

    }

    void Update()
    {
        int groundedSum = 0;

        for (int i = 0; i < groundCheckers.childCount; i++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(groundCheckers.GetChild(i).position.x, groundCheckers.GetChild(i).position.y, groundCheckers.GetChild(i).position.z), 0.09f, whatIsGround);
            groundedSum += (hitColliders.Length > 0) ? 1 : 0;
        }

        grounded = (groundedSum == 4);
        onAir= (groundedSum == 0);

        for (int i = 0; i < 2; i++)
        {
            float steering = 30f * Input.GetAxis("Horizontal");
            // wheels.transform.GetChild(i).localEulerAngles = Vector3.Slerp(wheels.transform.GetChild(i).localEulerAngles, new Vector3(90f, steering, 90f), Time.deltaTime * 5f);

            wheels.transform.GetChild(i).localRotation = Quaternion.Slerp(wheels.transform.GetChild(i).localRotation, Quaternion.Euler(0, steering, 90f), Time.deltaTime * 2f);
            // wheels.transform.GetChild(i).rotation = Quaternion.Euler(0, steering, 90f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < groundCheckers.childCount; i++)
        {
            Gizmos.DrawSphere(new Vector3(groundCheckers.GetChild(i).position.x, groundCheckers.GetChild(i).position.y, groundCheckers.GetChild(i).position.z), 0.09f);
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        // float desiredTurnSpeed = Mathf.Clamp((gearBox.speed + Mathf.Abs(gearBox.GetSidewaysFactor())) * 2f, 0f, turnSpeed);
        desiredTurnSpeed = Mathf.Lerp(desiredTurnSpeed, turnSpeed, .5f);

        if (Input.GetButton("Fire2"))
        {
            desiredTurnSpeed = turnSpeed * 2f;
        }

        if (grounded && (gearBox.speed > 0.1f || Mathf.Abs(gearBox.GetSidewaysFactor()) > 3f))
        {
            transform.Rotate(Vector3.up, desiredTurnSpeed * Input.GetAxis("Horizontal"), Space.World);
        }

        if (onAir == true)
        {
            rb.AddForce(Vector3.down * (rb.mass / 1000f) * 20f, ForceMode.Acceleration);
        }
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

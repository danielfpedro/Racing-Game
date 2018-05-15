using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {

    [Header("Engine")]
    public Engine engine;
    public int currentRpm;

    [Header("Gear Box")]
    public GearBox gearBox;
    private Gear currentGear;
    private int currentGearIndex;

    [HideInInspector]
    public Rigidbody rb;

    /*[Header("Outros")]
    public float drag = 1f;
    public Rigidbody rb;
    public float turnSpeed;
    public float grip;

    public float burnoutIntensity;
    public AudioSource engineSound;

    public bool grounded = false;
    public bool onAir = false;
    public LayerMask whatIsGround;

    public Transform groundCheckers;

    private float desiredTurnSpeed;

    public GameObject wheels;

    public float steeringAngle;

    private float lastSteering;
    */
    
    void Start () {
        rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
    }

    /*
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
            float steering = steeringAngle * Input.GetAxis("Horizontal");

            wheels.transform.GetChild(i).localRotation = Quaternion.Slerp(wheels.transform.GetChild(i).localRotation, Quaternion.Euler(0, steering, 90f), Time.deltaTime * 4f);
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

    void FixedUpdate ()
    {
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

    public float GetVelocityX()
    {
        Vector3 velocity = transform.InverseTransformDirection(rb.velocity);
        return velocity.x;
    }
    */
}

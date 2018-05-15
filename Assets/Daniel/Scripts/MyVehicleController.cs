using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVehicleController : MonoBehaviour {

    [Header("Engine")]
    public Engine engine;

    public float currentRpm;

    [Header("Gear Box")]
    public GearBox gearBox;
    private int currentGearIndex;
    private float lagCounter = 0f;

    [Header("Body")]
    public int weight;

    [Header("Steering")]
    public float turnSpeed;
    private float desiredTurn;

    [Header("Aerodinamics")]
    [Range(1, 10)]
    public float aerodinamics;
    public float sidewaysGrip;
    [HideInInspector]
    public float backwardForce;
    [HideInInspector]
    public float sidewaysCounterForce;

    [Header("Wheels")]
    public Transform wheels;
    // Cache on start since the car always will have
    // the same amount of wheels so I cant count child everytime
    private int wheelsCount;
    public int wheelsGroundedCount;
    public LayerMask whatIsGround;

    [HideInInspector]
    public Rigidbody rb;
    // [HideInInspector]
    public float throttle;
    public float forwardForce;

    void Start()
    {
        rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rb.mass = weight;

        currentGearIndex = 0;
        currentRpm = 0;
        forwardForce = 0f;

        lagCounter = 0f;

        wheelsCount = wheels.childCount;
        wheelsGroundedCount = 0;
    }


    // Update is called once per frame
    void Update () {
		
	}
    private void FixedUpdate()
    {
        throttle = Input.GetAxis("Throttle");

        if (GetSpeed() >= GetCurrentGear().maxSpeedToChange)
        {
            GearUp();
        }
        else if (GetSpeed() < (PreviousGear().maxSpeedToChange - 5f))
        {
            GearDown();
        }

        /*FORWARD MOVE*/
        PushForward();
        

        /*BREAKING*/
        float breakAxis = Input.GetAxis("Break");
        rb.AddRelativeForce(((Vector3.back * GetSpeed())) * breakAxis, ForceMode.Acceleration);

        /*STEERING*/
        desiredTurn = turnSpeed;

        if (GetSpeed() > 0)
        {
            transform.Rotate(Vector3.up, desiredTurn * Input.GetAxis("Horizontal"), Space.World);
        }

        /*Aerodinamics*/
        PullBackward();
        // Multiply sideways grip by 1000 so we can put a small number con sidewaysgruip setup
        sidewaysCounterForce = -GetSidewaysFactor() * (sidewaysGrip * 1000f);
        rb.AddForce(transform.right * sidewaysCounterForce);


        /*WHEELS*/
        wheelsGroundedCount = 0;
        for (int i = 0; i < wheels.childCount; i++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(wheels.GetChild(i).Find("GroundCheck").transform.position.x, wheels.GetChild(i).Find("GroundCheck").position.y, wheels.GetChild(i).Find("GroundCheck").position.z), 0.09f, whatIsGround);
            wheelsGroundedCount += (hitColliders.Length > 0) ? 1 : 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < wheels.childCount; i++)
        {
            Gizmos.DrawSphere(new Vector3(wheels.GetChild(i).Find("GroundCheck").transform.position.x, wheels.GetChild(i).Find("GroundCheck").position.y, wheels.GetChild(i).Find("GroundCheck").position.z), 0.09f);
        }
    }

    private void PushForward()
    {
        forwardForce = (GetTorqueFromPorcentage() / 5f) * throttle;
        if (Time.time <= lagCounter)
        {
            forwardForce *= .5f;
        }

        rb.AddRelativeForce(Vector3.forward * forwardForce, ForceMode.Acceleration);

    }
    private void PullBackward()
    {
        backwardForce = GetSpeed() / aerodinamics;
        rb.AddRelativeForce(Vector3.back * backwardForce, ForceMode.Acceleration);
    }
    private float GetTorqueFromPorcentage()
    {
        return (engine.horsePower * GetCurrentGear().torquePorcentage / 100f);
    }
    private Gear GetCurrentGear()
    {
        return gearBox.gears[currentGearIndex];
    }


    public void GearUp()
    {
        if (!IsLastGear())
        {
            currentGearIndex++;
            lagCounter = Time.time + gearBox.lag;
        }
    }
    public void GearDown()
    {
        if (!IsFirstGear())
        {
            currentGearIndex--;
        }
    }
    public bool IsLastGear()
    {
        return (currentGearIndex >= (gearBox.gears.Length - 1));
    }
    public bool IsFirstGear()
    {
        return (currentGearIndex <= 0);
    }
    public Gear PreviousGear()
    {
        int previousIndex = (currentGearIndex == 0) ? 0 : currentGearIndex - 1;
        return gearBox.gears[previousIndex];
    }
    public float GetSpeed()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        return Mathf.RoundToInt(Mathf.Abs(localVelocity.z * 1f));
    }

    public int GetCurrentGearNumber()
    {
        return currentGearIndex + 1;
    }
    public float GetSidewaysFactor()
    {
        float returnValue = transform.InverseTransformDirection(rb.velocity).x;
        if (returnValue >= -2 && returnValue <= 2f)
        {
            return 0f;
        }
        return transform.InverseTransformDirection(rb.velocity).x;
    }

    public bool IsOnAir()
    {
        return (wheelsGroundedCount == 0);
    }
}

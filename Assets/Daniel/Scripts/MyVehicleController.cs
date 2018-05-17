using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVehicleController : MonoBehaviour {

    [HideInInspector]
    public Rigidbody rb;

    [Header("Engine")]
    public Engine engine;
    public float forceOfTheGear;
    public float enginePitch;
    [HideInInspector]
    public AudioSource engineSoundSource;

    public float currentRpm;

    [Header("Gear Box")]
    public GearBox gearBox;
    private int currentGearIndex;
    private float lagCounter = 0f;

    [Header("Steering")]
    public float steeringAngle = 40f;
    public float turnSpeed;
    public float desiredTurn;

    [Header("Tyres")]
    [Range(0.1f, 1f)]
    public float forwardGrip;
    public float currentForwardGrip;
    public float burnoutIntensity;

    [Header("Body")]
    public int weight;
    [Range(1, 10)]
    public float aerodinamics;
    [Range(0.1f, 20f)]
    public float sidewaysGrip;
    [HideInInspector]
    public float backwardForce;
    [HideInInspector]
    public float sidewaysCounterForce;
    // public float maxSpeed;

    [Header("Wheels")]
    public Transform wheels;
    // Cache on start since the car always will have
    // the same amount of wheels so I cant count child everytime
    private int wheelsCount;
    [HideInInspector]
    public int wheelsGroundedCount;
    public LayerMask whatIsGround;
    public GameObject smokeRef;
    private List<ParticleSystem> smokes = new List<ParticleSystem>();
    
    [Header("Inspect")]
    public float throttle;
    public float forwardForce;
    // Total of wheels of the car less wheels on ground.
    // If we have a 4 wheels car with 0 wheels on ground it will be (4/0) wich will
    // result 0 so any forward force times 0 will be 0 wich will not apply any force
    public float wheelsOnGroundMultiplier;

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

        for (int i = 0; i < wheels.childCount; i++)
        {
            GameObject s = Instantiate(smokeRef, wheels.GetChild(i));
            smokes.Add(s.GetComponent<ParticleSystem>());
        }

        engineSoundSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        engineSoundSource.loop = true;
        engineSoundSource.clip = engine.sound;
        engineSoundSource.Play();
    }


    // Update is called once per frame
    void Update () {
		
	}
    private void FixedUpdate()
    {
        wheelsOnGroundMultiplier = wheelsGroundedCount / wheelsCount;
        // enginePitch = 1 - (GetCurrentGear().maxSpeedToChange - GetSpeed()) / (GetCurrentGear().maxSpeedToChange - ((currentGearIndex == 0) ? 0 : (PreviousGear().maxSpeedToChange + 1)));

        engineSoundSource.pitch = (GetSpeed() / 240) + (((1 - currentForwardGrip) * 1.5f) * throttle) + .6f;

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
        if (IsGrounded())
        {
            rb.AddRelativeForce((Vector3.back * GetSpeed()) * breakAxis, ForceMode.Acceleration);
        }
        

        /*STEERING*/
        

        if (GetSpeed() > 0 && IsGrounded())
        {

            desiredTurn = turnSpeed;

            if (Input.GetButton("HandBreak"))
            {
                desiredTurn = turnSpeed * 2f;
            }
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

        /*BURNOUT*/
        currentForwardGrip = forwardGrip + (GetSpeed() / 100f);
        currentForwardGrip = Mathf.Clamp(currentForwardGrip, 0.1f, 1f);
        // burnoutIntensity = (GetForwardForce() - (GetForwardForce() * currentForwardGrip)) * (wheelsGroundedCount / wheelsCount);
        burnoutIntensity = (GetForwardForce() * (1f - currentForwardGrip)) * (wheelsGroundedCount / wheelsCount);

        for (int i = 0; i < smokes.Count; i++)
        {
            var emission = smokes[i].emission;
            float emissionMultiplier = 20f;
            float forwardBurnoutFactor = burnoutIntensity;

            if (i < 2)
            {
                forwardBurnoutFactor = 0f;
            }
            emission.rateOverTime = ((forwardBurnoutFactor + GetSidewaysFactorToEmission()) * emissionMultiplier) * wheelsOnGroundMultiplier;
        }

        /*Push Down on Air*/
        DownForce();

        /*Steering Visual Wheels*/
        for (int i = 0; i < 2; i++)
        {
            float steering = steeringAngle * Input.GetAxis("Horizontal");

            wheels.transform.GetChild(i).localRotation = Quaternion.Slerp(wheels.transform.GetChild(i).localRotation, Quaternion.Euler(0, steering, 90f), Time.deltaTime * 4f);
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

    public bool IsGrounded()
    {
        return (wheelsGroundedCount == wheels.childCount);
    }

    public float GetSidewaysFactorToEmission()
    {
        return Mathf.Abs(GetSidewaysFactor() / 10f);
    }

    private void PushForward()
    {
        rb.AddRelativeForce(Vector3.forward * GetForwardForce(), ForceMode.Acceleration);

    }
    private float GetForwardForce()
    {
        forceOfTheGear = ((GetTorqueFromPorcentage() / 10f) - (weight / 1000f) * (aerodinamics / 10f) - (burnoutIntensity) * (wheelsGroundedCount / wheelsCount)) * currentForwardGrip * throttle;
        if (Time.time <= lagCounter)
        {
            forceOfTheGear *= .8f;
        }

        return forceOfTheGear;
    }
    private void DownForce()
    {
        if (IsOnAir())
        {
            rb.AddForce(Vector3.down * Mathf.Clamp((weight / 15f), 0f, 1000f), ForceMode.Acceleration);
        }
        
    }
    private void PullBackward()
    {
        if (throttle == 0 && IsGrounded())
        {
            backwardForce = GetSpeed() / 5f;
            rb.AddRelativeForce(Vector3.back * backwardForce, ForceMode.Acceleration);
        }
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
        return Mathf.RoundToInt(Mathf.Abs(localVelocity.z * 2f));
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

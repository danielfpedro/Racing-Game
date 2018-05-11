using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gear
{
    public float torque;
    public float rpm = 0f;
    private float minEngineSoundPitch = 0.2f;
}
public class GearBoxController : MonoBehaviour {

    public Rigidbody rb;

    public Gear[] gears;
    public Gear currentGear;

    public int currentGearIndex;
    public int currentGearNumber;

    public float throttle;
    public float torqueApplyed;
    public float speed;
    public AudioSource engineSound;
    public float maxRpm = 7000f;
    public float starterRpm = 5000f;
    public float minRpm = 3000f;

    public float velocityX;

    private VehicleController vehicleController;

    // Use this for initialization
    void Start () {
        StartEngine();
        vehicleController = GetComponent<VehicleController>();

	}
	public void StartEngine()
    {
        currentGearIndex = 0;
        currentGear = gears[0];
    }
	// Update is called once per frame
	void Update () {


        // float desiredPitch = currentGear.rpm / maxRpm;
        // desiredPitch = Mathf.Clamp(desiredPitch, currentGear.minEngineSoundPitch, 1f);
        // engineSound.pitch = desiredPitch;
    }
    private void FixedUpdate()
    {
        throttle = Input.GetAxis("Throttle");

        torqueApplyed = currentGear.torque * throttle;
        float desiredRpm = (torqueApplyed - vehicleController.drag) * 5f;

        float lastRpm = currentGear.rpm;
        currentGear.rpm += desiredRpm;

        currentGear.rpm = Mathf.Clamp(currentGear.rpm, 0f, maxRpm);

        if (currentGear.rpm >= maxRpm)
        {
            GearUp();
        }
        else if (currentGearIndex > 0 && currentGear.rpm <= minRpm && currentGear.rpm < lastRpm)
        {
            GearDown();
        }

        currentGearNumber = currentGearIndex + 1;

        velocityX = vehicleController.GetVelocityX();

        speed = GetSpeed();
        if (currentGear.rpm < maxRpm)
        {
            Move(torqueApplyed);
        } else
        {
            Move(vehicleController.drag);
        }
    }

    public float GetSpeed()
    {
        // return Mathf.RoundToInt(Mathf.Clamp(rb.velocity.magnitude * 3.6f, 0f, 10000f));
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        return Mathf.RoundToInt(Mathf.Clamp(localVelocity.z / 3.6f, 0f, 1000000000f));
    }
    public void GearUp()
    {
        if (!IsLastGear())
        {
            currentGearIndex++;
            currentGear = gears[currentGearIndex];
            currentGear.rpm = starterRpm;
        }
    }
    public void GearDown()
    {
        if (!IsFirstGear())
        {
            currentGearIndex--;
            currentGear = gears[currentGearIndex];
            currentGear.rpm = starterRpm;
        }
    }
    public bool IsLastGear()
    {
        return (currentGearIndex >= (gears.Length - 1));
    }
    public bool IsFirstGear()
    {
        return (currentGearIndex <= 0);
    }

    void Move(float torque)
    {
        rb.AddRelativeForce(Vector3.forward * torque, ForceMode.Acceleration);
    }
}

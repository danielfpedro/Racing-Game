using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SGear
{
    public float torque;
    public int maxSpeed;
}
public class SimpleGearBoxController : MonoBehaviour {

    public Rigidbody rb;

    public SGear[] gears;
    public SGear currentGear;

    public int currentGearIndex;
    public int currentGearNumber;

    public float throttle;
    public float speed;
    public float desiredTorque = 0f;

    public ParticleSystem[] tyreSmokes;
    public bool smokePlaying = false;

    [Header("Terrain Grip")]
    [Range(0.1f, 1f)]
    public float forwardGrip = 0.5f;
    public float currentForwardGrip = 0f;
    public float burnoutIntensity = 0f;


    // Use this for initialization
    void Start () {
        currentGearIndex = 0;
        currentGearNumber = 1;
        currentGear = gears[currentGearIndex];

        foreach(ParticleSystem tyreSmoke in tyreSmokes)
        {
            tyreSmoke.Pause();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void FixedUpdate()
    {
        speed = GetSpeed();
        throttle = Input.GetAxis("Throttle");

        desiredTorque = (IsLastGear() && speed >= currentGear.maxSpeed) ? 0f : currentGear.torque * throttle;

        // Forward Grip
        currentForwardGrip = forwardGrip + (speed / 110f);
        currentForwardGrip = Mathf.Clamp(currentForwardGrip, 0.1f, 1f);
        burnoutIntensity = desiredTorque - (desiredTorque * currentForwardGrip);

        desiredTorque = Mathf.Clamp(desiredTorque - (burnoutIntensity * 1.2f), 0f, 10000f);

        // Debug.Log("Previos " + PreviousGear().maxSpeed + " | CurrentMaxSpeed " + currentGear.maxSpeed + " | Speed" + speed);
        if (speed >= currentGear.maxSpeed)
        {
            GearUp();
        }
        else if (speed < PreviousGear().maxSpeed)
        {
            GearDown();
        }

        if (burnoutIntensity > 0 && !tyreSmokes[0].isPlaying)
        {
            foreach (ParticleSystem tyreSmoke in tyreSmokes)
            {
                tyreSmoke.Play();
            }
        } else {
            foreach (ParticleSystem tyreSmoke in tyreSmokes)
            {
                tyreSmoke.Pause();
            }
        }
        Move(desiredTorque);
    }

    public float GetSpeed()
    {
        // return Mathf.RoundToInt(Mathf.Clamp(rb.velocity.magnitude * 3.6f, 0f, 10000f));
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        return Mathf.RoundToInt(Mathf.Clamp(localVelocity.z * 1.4f, 0f, 1000000000f));
    }
    public void GearUp()
    {
        if (!IsLastGear())
        {
            currentGearIndex++;
            currentGear = gears[currentGearIndex];

            currentGearNumber = currentGearIndex + 1;
        }
    }
    public void GearDown()
    {
        if (!IsFirstGear())
        {
            currentGearIndex--;
            currentGear = gears[currentGearIndex];

            currentGearNumber = currentGearIndex + 1;
        }
    }
    public SGear PreviousGear()
    {
        int previousIndex = (currentGearIndex == 0) ? 0 : currentGearIndex - 1;
        return gears[previousIndex];
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

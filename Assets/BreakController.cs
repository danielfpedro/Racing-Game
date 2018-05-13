using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakController : MonoBehaviour {

    private SimpleGearBoxController gearBox;
    public float breakAxis = 0f;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
		gearBox = GetComponent<SimpleGearBoxController>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        breakAxis = Input.GetAxis("Break");

        rb.AddRelativeForce(((Vector3.back * gearBox.speed)) * breakAxis, ForceMode.Acceleration);
    }
}

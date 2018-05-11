using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelemetryCanvasController : MonoBehaviour {

    public GameObject target;
    public Text AcellTorque;
    public Text CarWeight;
    public Text forwardGrip;
    public Text currentForwardGrip;
    public Text burnoutIntensity;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AcellTorque.text = "Forward Force: " + target.GetComponent<SimpleGearBoxController>().desiredTorque.ToString();
        CarWeight.text = "Vehicle Weight: " + target.GetComponent<SimpleGearBoxController>().rb.mass;
        forwardGrip.text = "Forward Grip: " + target.GetComponent<SimpleGearBoxController>().forwardGrip;
        currentForwardGrip.text = "Current Forward Grip: " + target.GetComponent<SimpleGearBoxController>().currentForwardGrip;
        burnoutIntensity.text = "Burnout Intensity: " + target.GetComponent<SimpleGearBoxController>().burnoutIntensity;
    }
}

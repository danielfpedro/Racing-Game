using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelemetryCanvasController : MonoBehaviour {

    public GameObject target;
    public Text forwardForce;
    public Text backwardForce;
    public Text CarWeight;
    public Text pushingForward;
    public Text forwardGrip;
    public Text currentForwardGrip;
    public Text burnoutIntensity;
    public Text sidewaysFactor;
    public Text grounded;
    public Text onAir;
    public Text sidewaysCounterForce;
    public Text wheelsGroundedCount;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        forwardForce.text = "Forward Force: " + target.GetComponent<MyVehicleController>().forwardForce;
        backwardForce.text = "Backward Force: " + target.GetComponent<MyVehicleController>().backwardForce;
        CarWeight.text = "Vehicle Weight: " + target.GetComponent<MyVehicleController>().rb.mass;
        sidewaysFactor.text = "Sideways Factor: " + target.GetComponent<MyVehicleController>().GetSidewaysFactor();
        sidewaysCounterForce.text = "Sideways Counter Force: " + target.GetComponent<MyVehicleController>().sidewaysCounterForce;
        wheelsGroundedCount.text = "Wheels Grounded Count: " + target.GetComponent<MyVehicleController>().wheelsGroundedCount;
        onAir.text = "On Air: " + target.GetComponent<MyVehicleController>().IsOnAir();
        /*
        
        forwardGrip.text = "Forward Grip: " + target.GetComponent<SimpleGearBoxController>().forwardGrip;
        currentForwardGrip.text = "Current Forward Grip: " + target.GetComponent<SimpleGearBoxController>().currentForwardGrip;
        burnoutIntensity.text = "Burnout Intensity: " + target.GetComponent<SimpleGearBoxController>().burnoutIntensity;
        grounded.text = "Grounded: " + target.GetComponent<VehicleController>().grounded;
        
        pushingForward.text = "Pushing Forward: " + target.GetComponent<SimpleGearBoxController>().pushingForward;*/
    }
}

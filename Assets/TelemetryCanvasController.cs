using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelemetryCanvasController : MonoBehaviour {

    public GameObject target;

    public Text engineHp;
    public Text weight;
    public Text forceOfTheGear;


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
        weight.text = "Weight: " + target.GetComponent<MyVehicleController>().weight;
        engineHp.text = "Engine HP: " + target.GetComponent<MyVehicleController>().engine.horsePower;
        forceOfTheGear.text = "Force of the Gear: " + target.GetComponent<MyVehicleController>().forceOfTheGear;
        burnoutIntensity.text = "Burnout Intensity: " + target.GetComponent<MyVehicleController>().burnoutIntensity;
        forwardGrip.text = "Forward Grip: " + target.GetComponent<MyVehicleController>().forwardGrip;
        currentForwardGrip.text = "Current Forward Grip: " + target.GetComponent<MyVehicleController>().currentForwardGrip;

        backwardForce.text = "Backward Force: " + target.GetComponent<MyVehicleController>().backwardForce;
        CarWeight.text = "Vehicle Weight: " + target.GetComponent<MyVehicleController>().rb.mass;
        sidewaysFactor.text = "Sideways Factor: " + target.GetComponent<MyVehicleController>().GetSidewaysFactor();
        sidewaysCounterForce.text = "Sideways Counter Force: " + target.GetComponent<MyVehicleController>().sidewaysCounterForce;
        wheelsGroundedCount.text = "Wheels Grounded Count: " + target.GetComponent<MyVehicleController>().wheelsGroundedCount;
        onAir.text = "On Air: " + target.GetComponent<MyVehicleController>().IsOnAir();
        /*
        currentForwardGrip.text = "Current Forward Grip: " + target.GetComponent<SimpleGearBoxController>().currentForwardGrip;
        grounded.text = "Grounded: " + target.GetComponent<VehicleController>().grounded;
        
        pushingForward.text = "Pushing Forward: " + target.GetComponent<SimpleGearBoxController>().pushingForward;*/
    }
}

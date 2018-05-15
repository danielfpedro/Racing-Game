using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceCanvasController : MonoBehaviour {

    public GameObject target;

    public Text rpm;
    public Text speed;
    public Text gear;


    private void Update()
    {
        rpm.text = "RPM: " + target.GetComponent<MyVehicleController>().currentRpm.ToString();
        gear.text = "Gear: " + target.GetComponent<MyVehicleController>().GetCurrentGearNumber().ToString();
        speed.text = "Speed: " + target.GetComponent<MyVehicleController>().GetSpeed().ToString();
    }
}

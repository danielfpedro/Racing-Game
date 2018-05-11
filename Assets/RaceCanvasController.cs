using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceCanvasController : MonoBehaviour {

    public GameObject target;

    public Text speed;
    public Text gear;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        // rpm.text = target.GetComponent<SimpleGearBoxController>().currentGear.rpm.ToString("f0");
        speed.text = target.GetComponent<SimpleGearBoxController>().GetSpeed().ToString() + " KM/H";
        gear.text = "Gear: " + target.GetComponent<SimpleGearBoxController>().currentGearNumber.ToString();
    }
}

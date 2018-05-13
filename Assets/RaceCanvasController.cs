using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceCanvasController : MonoBehaviour {

    public GameObject target;

    public Text speed;
    public Text gear;


    public Vector3 lastTargetPos;
    // Use this for initialization
    void Start () {
        // speed.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        // lastTargetPos = speed.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        //float smooth = .001f;

        // Vector3 targetPos = Camera.main.WorldToScreenPoint(target.transform.position);
        // speed.transform.position = Vector3.Lerp(lastTargetPos, new Vector3(targetPos.x - 50f, targetPos.y + 160f, targetPos.z), smooth);
        // lastTargetPos = speed.transform.position;

        speed.text = target.GetComponent<SimpleGearBoxController>().GetSpeed().ToString() + " KM/H";
        gear.text = "Gear: " + target.GetComponent<SimpleGearBoxController>().currentGearNumber.ToString();
    }
}

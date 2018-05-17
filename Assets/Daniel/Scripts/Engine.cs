using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "VehicleParts/Engine")]
public class Engine : ScriptableObject {

    public int horsePower;
    public int rpmLimit;
    public AudioClip sound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

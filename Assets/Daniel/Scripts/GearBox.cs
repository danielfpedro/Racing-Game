using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gear
{
    public int torquePorcentage;
    public int maxSpeedToChange;
}
[CreateAssetMenu(fileName = "New GearBox", menuName = "VehicleParts/GearBox")]
public class GearBox : ScriptableObject {

    // public int maxRpm;
    // public int startingRpmGear;
    // public int minRpmToChangeGear;

    [Range(0.1f, 1f)]
    public float lag; // In ms
    public Gear[] gears;
}

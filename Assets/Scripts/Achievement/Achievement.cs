using UnityEngine;
using System.Collections;

public abstract class Achievement : MonoBehaviour {
    public bool meets_conditions;
    public bool MeetsCondition { get { return meets_conditions; } }
    public int myID;
    public int iosID;
    public int AndroidID;
    public bool oneRun; // that have to be achieved in one run
    public bool dirty;
    public float progress;
    public bool granted;
}

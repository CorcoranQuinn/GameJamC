using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    private bool Pressed;
    private bool Up;
    private Vector3 Proportions;
    private float StartTime;
    private float GateSpeed;
    private float Journey;
    private float DistCovered;
    private float FractionTraveled;
    private Vector3 UpMarker;
    private Vector3 DownMarker;
	// Use this for initialization
	void Start () {
        Pressed = false;
        Up = false;
        Proportions = new Vector3(0,1,0);
        GateSpeed = .3f;
        UpMarker = transform.position + Proportions;
        DownMarker = transform.position;
	}
    public void isPressed(bool pressing)
    {
        Pressed = pressing;      
        StartTime = Time.time;
        Journey = Vector3.Distance(DownMarker, UpMarker);
    }
    public void raiseDoor()
    {
        transform.position = Vector3.Lerp(DownMarker, UpMarker, FractionTraveled);
        if (transform.position == UpMarker)
        {
            Up = true;
        }
    }
    public void lowerDoor()
    {
        transform.position = Vector3.Lerp(UpMarker, DownMarker, FractionTraveled);
        if (transform.position == DownMarker)
        {
            Up = false;
        }
    }
    // Update is called once per frame
    void Update () {
        //If the door is down and the button is being pressed raise the door by lerping it
        if (Pressed == true && Up == false)
        { 
            DistCovered = (Time.time - StartTime) * GateSpeed;
            FractionTraveled = DistCovered / Journey;
            raiseDoor();
           
        }
        //If the door is up and the button is no longer being pressed lower the door back to its original position
        if (Pressed == false && Up == true)
        {
            DistCovered = (Time.time - StartTime) * GateSpeed;
            FractionTraveled = DistCovered / Journey;
            lowerDoor();
        }
        
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    public GameObject TriggeredObject;
    private float MaxDistance;
    public LayerMask ButtonMask = -1;
    private bool pressed;
    // Use this for initialization
    void Start () {
        MaxDistance = 1;
	}
    public void FixedUpdate()
    {
        bool OnTop = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), MaxDistance, ButtonMask.value, QueryTriggerInteraction.Ignore);
        if (!pressed && OnTop)
        {
            Debug.Log("hey");
            TriggeredObject.GetComponent<Gate>().isPressed(true);
            pressed = true;
        }
        if (pressed && !OnTop)
        {
            Debug.Log("I stepped off");
            TriggeredObject.GetComponent<Gate>().isPressed(false);
            pressed = false;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}

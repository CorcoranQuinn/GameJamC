using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Button trigger;

    public bool moving = false;

    public Vector3 TargetPosition;
    public float Speed = 5;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        float step = Speed * Time.deltaTime;

        Vector3 target = trigger.Pressed ? TargetPosition : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}

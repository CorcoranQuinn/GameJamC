using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool Pressed;

    void FixedUpdate()
    {
        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(transform.position, 0.1f, Vector3.up, out hitInfo);

        Pressed = hit;
    }
}

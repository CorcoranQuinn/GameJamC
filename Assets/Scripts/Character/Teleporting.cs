using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControllerInputProvider))]
[RequireComponent(typeof(CharacterMovement))]
public class Teleporting : MonoBehaviour
{
    public float MaxDistance = 30;
    public float minDistance = 2;
    public float TeleportSpeed = 20f;

    public ParticleSystem IndicatorEffect;
    public LayerMask PlayerMask = -1;

    private CharacterMovement defaultMovement;
    private Transform playerCamera;
    private CharacterInputProvider inputProvider;
    private ParticleSystem indicator = null;

    private bool teleport;
    private Vector3 target;

    public Vector3 heightOffset = new Vector3(0, 0.5f, 0);

    // Use this for initialization
    void Start()
    {
        inputProvider = GetComponent<CharacterInputProvider>();

        defaultMovement = GetComponent<CharacterMovement>();
        playerCamera = defaultMovement.PlayerCamera;
    }

    // FixedUpdate is called 60 times per second
    private void FixedUpdate()
    {
        Vector3 lookDirection = playerCamera.TransformDirection(Vector3.forward);
        RaycastHit teleRay;
        bool hit = Physics.Raycast(transform.position, lookDirection, out teleRay, MaxDistance, PlayerMask.value, QueryTriggerInteraction.Ignore);;

        if ((!hit || !(inputProvider.Teleport || inputProvider.TeleportUp)) && indicator != null)
        {
            Destroy(indicator.gameObject);
            indicator = null;
        }
        else if (hit && inputProvider.Teleport && !teleport)
        {
            if (indicator == null)
            {
                indicator = Instantiate(IndicatorEffect, teleRay.point, new Quaternion());
            }
            else
            {
                indicator.transform.position = teleRay.point;
            }
        }
        else if (hit && inputProvider.TeleportUp)
        {
            if (indicator != null)
            {
                Destroy(indicator.gameObject);
                indicator = null;
            }

            teleport = true;
            // defaultMovement.AbilityLockout = true;
        }
    }
    private void Update() { }
}

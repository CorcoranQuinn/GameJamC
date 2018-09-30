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

    private CharacterMovement characterMovement;
    private Transform playerCamera;
    private CharacterInputProvider inputProvider;
    private ParticleSystem indicator = null;

    private bool teleport;
    private Vector3 target;

    private Vector3 heightOffset = new Vector3(0, 1, 0);

    // Use this for initialization
    void Start()
    {
        inputProvider = GetComponent<CharacterInputProvider>();

        characterMovement = GetComponent<CharacterMovement>();
        playerCamera = characterMovement.PlayerCamera;
    }

    // FixedUpdate is called 60 times per second
    private void FixedUpdate()
    {
        Vector3 lookDirection = playerCamera.TransformDirection(Vector3.forward);
        RaycastHit teleRay;
        bool hit = Physics.Raycast(transform.position, lookDirection, out teleRay, MaxDistance, PlayerMask.value, QueryTriggerInteraction.Ignore);

        if ((!hit || !(inputProvider.Teleport || inputProvider.TeleportUp)) && indicator != null)
        {
            Destroy(indicator.gameObject);
            indicator = null;
        }
        else if (hit && inputProvider.Teleport && !teleport)
        {
            if (indicator == null)
            {
                indicator = Instantiate(IndicatorEffect, teleRay.point, Quaternion.Euler(0, 0, 0));
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

            target = teleRay.point + heightOffset;
            teleport = true;
            characterMovement.AbilityLockout = true;
        }
    }
    private void Update()
    {
        if (teleport)
        {
            Vector3 path = target - transform.position;
            Vector3 velocity = path.normalized * TeleportSpeed;

            Debug.Log("Position: " + transform.position.ToString());
            Debug.Log("Target: " + target.ToString());
            characterMovement.characterController.Move(velocity * Time.deltaTime);

            if (characterMovement.characterController.isGrounded)
            {
                teleport = false;
                characterMovement.AbilityLockout = false;
            }
        }
    }
}

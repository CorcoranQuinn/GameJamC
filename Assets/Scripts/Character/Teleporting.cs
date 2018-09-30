using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ControllerInputProvider))]
[RequireComponent(typeof(CharacterMovement))]
public class Teleporting : MonoBehaviour
{
    public float MaxDistance = 600;
    public LayerMask PlayerMask = -1;
    private GameObject OtherObject;
    private Collider OtherCollider;
    public ParticleSystem ParticleEffect;
    private ParticleSystem indicator;
    public float TeleSpeed = 0.5f;
    private RaycastHit teleRay;
    private float Journey;
    private float StartTime;
    private Transform StartMarker;
    private Transform EndMarker;
    private bool Teleport;
    private Vector3 inGround;

    private CharacterMovement defaultMovement;
    private CharacterInputProvider inputProvider;
    public Transform PlayerCamera;

    // Use this for initialization
    void Start()
    {
        inGround = new Vector3(1, 2, 1);
        inputProvider = GetComponent<CharacterInputProvider>();
        defaultMovement = GetComponent<CharacterMovement>();
    }

    // FixedUpdate is called 60 times per second
    private void FixedUpdate()
    {
        if (inputProvider.Teleport && indicator == null)
        {
            Vector3 direction = PlayerCamera.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, direction, out teleRay, MaxDistance, PlayerMask.value, QueryTriggerInteraction.Ignore))
            {
                indicator = Instantiate(ParticleEffect, teleRay.point, new Quaternion());
                OtherObject = teleRay.collider.gameObject;
                StartMarker = gameObject.GetComponent<Transform>();
                EndMarker = OtherObject.GetComponent<Transform>();
            }
        }
        if (inputProvider.TeleportUp && indicator != null)
        {
            Journey = Vector3.Distance(StartMarker.position, EndMarker.position);
            StartTime = Time.time;
            Destroy(indicator.gameObject);
            indicator = null;
            Teleport = true;
            defaultMovement.AbilityLockout = true;
        }
    }
    private void Update()
    {
        if (Teleport == true)
        {
            float DistCovered = (Time.time - StartTime) * TeleSpeed;
            float FractionTraveled = DistCovered / Journey;
            transform.position = Vector3.Lerp(StartMarker.position, EndMarker.position + inGround, FractionTraveled);
            if (transform.position == EndMarker.position + inGround)
            {
                Teleport = false;
                defaultMovement.AbilityLockout = false;
            }
        }

    }
}

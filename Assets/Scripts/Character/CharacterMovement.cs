using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterInputProvider))]
public class CharacterMovement : MonoBehaviour
{
    // Components
    private CharacterController characterController;
    private Collider _collider;
    private CharacterInputProvider inputProvider;

    // Inspector Settings -----------------------------------------------------
    [Serializable]
    public class MovementSettings
    {
        // Ground Movement
        public float ForwardSpeed = 10.0f; // Speed when walking forward
        public float BackwardSpeed = 4.0f; // Speed when walking backwards
        public float StrafeSpeed = 4.0f; // Speed when walking sideways
        public float MoveForce = 20f;
        public float Friction = 20f;

        // Air movement
        [Range(0f, 1f)] public float AirMovementMultiplier = 0.65f; // multiplies the movement force when in the air

        // Jump parameters
        private float jumpHeight = 3f;
        private float jumpDistance = 10f;

        [Range(0.1f, 1f)] public float MinimumJumpHeight = 0.5f;

        // Runtime attribute modifiers
        public float SpeedMod { get; set; } = 1;

        private float jumpHeightMod = 1;
        private float jumpDistanceMod = 1;

        public float JumpHeightMod
        {
            get { return jumpHeightMod; }

            set
            {
                jumpHeightMod = value;
                SetJumpConstants();
            }
        }
        public float JumpDistanceMod
        {
            get { return jumpDistanceMod; }

            set
            {
                jumpDistanceMod = value;
                SetJumpConstants();
            }
        }

        public float JumpHeight
        {
            get { return jumpHeight; }
            set { JumpHeightMod = value / jumpHeight; }
        }
        public float JumpDistance
        {
            get { return jumpDistance; }
            set { JumpDistanceMod = value / jumpDistance; }
        }

        // Real jump values
        public float JumpForce { get; private set; }
        public float Gravity { get; private set; }
        public float VariableJumpGravity
        {
            get
            {
                return Gravity / MinimumJumpHeight;
            }
        }

        private void SetJumpConstants()
        {
            float height = jumpHeightMod * jumpHeight;
            float distance = jumpDistanceMod * jumpDistance;

            JumpForce = 2 * height * ForwardSpeed / (distance / 2);
            Gravity = 2 * height * Mathf.Pow(ForwardSpeed, 2) / Mathf.Pow(distance / 2, 2);
        }

        public void ResetJumpConstants()
        {
            jumpHeightMod = 1;
            jumpDistanceMod = 1;
            SetJumpConstants();
        }

        public MovementSettings()
        {
            ResetJumpConstants();
        }
    }
    public MovementSettings Movement = new MovementSettings();
    // ------------------------------------------------------------------------

    // Accessors
    public bool IsGrounded { get { return characterController.isGrounded; } }

    public bool HandleMovement = true;

    // State
    private Vector3 playerVelocity = new Vector3(0, 0, 0);
    private bool shortJump = false;

    void Start()
    {
        _collider = GetComponent<Collider>();

        inputProvider = GetComponent<CharacterInputProvider>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (HandleMovement)
        {
            Vector2 input = getInput();
            horizontalMove(input);

            verticalMove();

            Move(playerVelocity * Time.deltaTime);
        }
    }

    public void Move(Vector3 velocity)
    {
        characterController.Move(velocity);
    }

    private void horizontalMove(Vector2 input)
    {
        if (input.magnitude > 1)
            input.Normalize();

        Vector3 localVelocity = transform.InverseTransformDirection(playerVelocity);
        Vector2 groundVelocity = (new Vector2(localVelocity.x, localVelocity.z));

        Vector2 velocityCap = new Vector2(
            Movement.StrafeSpeed,
            input.y > 0 ?
            Movement.ForwardSpeed :
            Movement.BackwardSpeed
        );

        Vector2 desiredVelocity = (input * velocityCap);

        Vector2 calcVelocity = applyForces(groundVelocity, desiredVelocity);

        Debug.Log("Speed: " + groundVelocity.magnitude);
        // Debug.Log("Local Velocity: " + groundVelocity);

        playerVelocity = transform.TransformDirection(new Vector3(calcVelocity.x, playerVelocity.y, calcVelocity.y));
    }

    private Vector2 applyForces(Vector2 current, Vector2 desired)
    {
        return new Vector2(applyForces(current.x, desired.x), applyForces(current.y, desired.y));
    }

    private float applyForces(float current, float desired)
    {
        bool applyFriction = IsGrounded && Math.Sign(desired) != Math.Sign(current);

        if (applyFriction)
        {
            current = Math.Sign(current) * Math.Max(Math.Abs(current) - Movement.Friction * Time.deltaTime, 0);
        }

        bool applyForce = Math.Sign(desired) != 0;

        if (applyForce)
        {
            float force = Movement.MoveForce * (IsGrounded ? 1 : Movement.AirMovementMultiplier);
            current = current + Math.Sign(desired) * force * Time.deltaTime;

            if (Math.Abs(current) > Math.Abs(desired) &&
                Math.Sign(current) == Math.Sign(desired))
            {
                current = desired;
            }
        }

        return current;
    }

    private void verticalMove()
    {
        if (IsGrounded)
        {
            playerVelocity = new Vector3(
                playerVelocity.x,
                inputProvider.JumpPressed ? Movement.JumpForce : 0,
                playerVelocity.z
            );

            shortJump = false;
        }
        else
        {
            shortJump = shortJump || playerVelocity.y > 0 && !inputProvider.Jump;
        }

        float gravity = shortJump ? Movement.VariableJumpGravity : Movement.Gravity;

        playerVelocity -= new Vector3(0, gravity * Time.deltaTime, 0);
    }

    private Vector2 getInput()
    {
        Vector2 input = inputProvider.Movement;

        return input;
    }
}

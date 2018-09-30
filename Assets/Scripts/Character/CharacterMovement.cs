using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ControllerInputProvider))]
public class CharacterMovement : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        // Ground Movement
        public float ForwardSpeed = 10.0f; // Speed when walking forward
        public float BackwardSpeed = 4.0f; // Speed when walking backwards
        public float StrafeSpeed = 4.0f; // Speed when walking sideways
        public float SpeedMod { get; set; } = 1;
        public float walkForce = 20f;
        public float Friction = 20f;

        // Air movement
        public float AirMovementMultiplier = 0.5f; // multiplies the movement force when in the air

        // Jump parameters
        private float jumpHeight = 5f;
        private float jumpDistance = 10f;

        public float JumpHeight
        {
            get { return jumpHeight; }

            set
            {
                JumpHeightMod = value / jumpHeight;
                SetJumpConstants();
            }
        }
        public float JumpDistance
        {
            get { return jumpDistance; }

            set
            {
                JumpDistanceMod = value / jumpDistance;
                SetJumpConstants();
            }
        }

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

        // Real jump values
        public float JumpForce { get; private set; }
        public float Gravity { get; private set; }

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

    public Transform PlayerCamera;
    public float PlayerViewYOffset = 0.6f;

    public MovementSettings movementSettings = new MovementSettings();
    public MouseLook mouseLook = new MouseLook();

    // TODO: should probably be moved to an input component
    public bool LockCursor = true;

    private CharacterController characterController;
    private CharacterInputProvider inputProvider;
    private CapsuleCollider capsule;
    private float yRotation;

    private Vector3 velocity = Vector3.zero;
    private bool jump, previouslyGrounded, jumping, isGrounded;

    [HideInInspector] public bool AbilityLockout = false;

    // Use this for initialization
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();

        inputProvider = GetComponent<CharacterInputProvider>();
        characterController = GetComponent<CharacterController>();

        mouseLook.Init(transform, PlayerCamera, inputProvider);
        updateCamera();
    }

    // Update is called once per frame
    void Update()
    {
        // Lock cursor to game window
        Cursor.lockState = LockCursor ? CursorLockMode.Locked : CursorLockMode.None;

        if (!AbilityLockout)
        {
            Vector2 input = getInput();
            horizontalMove(input);

            verticalMove();

            characterController.Move(velocity * Time.deltaTime);
        }

        updateCamera();
    }

    private void verticalMove()
    {
        if (characterController.isGrounded)
        {
            if (inputProvider.Jump)
            {
                velocity = new Vector3(velocity.x, movementSettings.JumpForce, velocity.z);
            }
            else
            {
                velocity = new Vector3(velocity.x, 0, velocity.z);
            }
        }

        velocity -= new Vector3(0, movementSettings.Gravity * Time.deltaTime, 0);
    }

    private void updateCamera()
    {
        mouseLook.LookRotation(transform, PlayerCamera);

        PlayerCamera.transform.position = new Vector3(
            transform.position.x,
            transform.position.y + PlayerViewYOffset,
            transform.position.z
        );

        PlayerCamera.transform.rotation = Quaternion.Euler(
            PlayerCamera.transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            0
        );
    }

    private Vector2 getInput()
    {
        Vector2 input = inputProvider.Movement;

        return input;
    }

    private void horizontalMove(Vector2 input)
    {
        if (input.magnitude > 1)
            input.Normalize();

        Vector2 desiredVelocity = input * new Vector2(
            movementSettings.StrafeSpeed,
            input.y > 0 ?
            movementSettings.ForwardSpeed :
            movementSettings.BackwardSpeed
        );

        velocity = transform.TransformDirection(new Vector3(desiredVelocity.x, velocity.y, desiredVelocity.y));
    }
}

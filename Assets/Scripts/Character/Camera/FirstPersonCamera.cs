using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterInputProvider))]
public class FirstPersonCamera : MonoBehaviour
{
	// Components
	private CharacterInputProvider inputProvider;

	// Mouse Settings
	private MouseLook Mouse = new MouseLook();
	private bool LockCursor = true;

	// Inspector Settings
	public Camera Cam;
	public float PlayerViewYOffset = 1f;

	// Accessors
	public Vector3 CameraPosition
	{
		get
		{
			Vector2 crosshairPos = new Vector2(Screen.width / 2, Screen.height / 2);

			return Cam.ScreenToWorldPoint(
				new Vector3(
					crosshairPos.x,
					crosshairPos.y,
					Cam.nearClipPlane
				)
			);
		}
	}

	public Vector3 CameraDirection
	{
		get { return Cam.transform.forward; }
	}

	public void Start()
	{
		inputProvider = GetComponent<CharacterInputProvider>();

		Mouse.Init(transform, Cam.transform, inputProvider);
	}

	public void Update()
	{
		// Lock cursor to game window
		if (LockCursor)
		{
			Cursor.lockState = LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = false;
		}

		// Move
		Mouse.LookRotation(transform, Cam.transform);

		Cam.transform.position = new Vector3(
			transform.position.x,
			transform.position.y + PlayerViewYOffset,
			transform.position.z
		);

		Cam.transform.rotation = Quaternion.Euler(
			Cam.transform.rotation.eulerAngles.x,
			transform.rotation.eulerAngles.y,
			0
		);
	}
}

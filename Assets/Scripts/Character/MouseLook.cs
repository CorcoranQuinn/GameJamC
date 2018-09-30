//Based heavily on the Unity Standard Asset FPS character controller

using System;
using UnityEngine;

[Serializable]
public class MouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    private CharacterInputProvider inputProvider;

    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;

    public void Init(Transform character, Transform camera, CharacterInputProvider inputProvider)
    {
        characterTargetRot = character.localRotation;
        cameraTargetRot = camera.localRotation;
        this.inputProvider = inputProvider;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        Vector2 lookInput = inputProvider.Look;

        characterTargetRot *= Quaternion.Euler(0f, lookInput.x, 0f);
        cameraTargetRot *= Quaternion.Euler(-lookInput.y, 0f, 0f);

        if (clampVerticalRotation)
            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = characterTargetRot;
            camera.localRotation = cameraTargetRot;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}

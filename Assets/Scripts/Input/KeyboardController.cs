using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyboardController", menuName = "Controllers/Keyboard")]
public class KeyboardController : Controller
{
    //I'm using the Unity Axis system because there isn't an easy way to get Mouse position delta
    public string MouseXAxis = "Mouse X";
    public string MouseYAxis = "Mouse Y";
    public float LookSensitivity = 1;
    public KeyCode JumpKey = KeyCode.Space;

    public override Vector2 MoveStick
    {
        get
        {
            Vector2 moveVec = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                moveVec.y += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveVec.y -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveVec.x += 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveVec.x -= 1;
            }
            return moveVec;
        }
    }

    public override Vector2 LookStick => new Vector2(Input.GetAxis(MouseXAxis), Input.GetAxis(MouseYAxis)) * LookSensitivity;

    public override bool Teleport => Input.GetMouseButton(0);

    public override bool TeleportUp => Input.GetMouseButtonUp(0);

    public override bool Jump => Input.GetKeyDown(JumpKey);
}

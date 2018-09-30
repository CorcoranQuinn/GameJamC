using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputProvider : CharacterInputProvider
{
    public Controller Controller;

    public override Vector2 Movement => Controller.MoveStick;

    public override Vector2 Look => Controller.LookStick;

    public override bool Teleport => Controller.Teleport;

    public override bool TeleportUp => Controller.TeleportUp;

    public override bool Jump => Controller.Jump;
}

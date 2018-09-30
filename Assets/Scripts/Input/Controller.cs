using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : ScriptableObject
{
    //Game Controlls
    public abstract Vector2 MoveStick { get; }
    public abstract Vector2 LookStick { get; }
    public abstract bool Teleport { get; }
    public abstract bool TeleportUp { get; }
    public abstract bool Jump { get; }
}

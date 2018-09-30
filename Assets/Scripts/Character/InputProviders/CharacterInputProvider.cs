using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterInputProvider : MonoBehaviour
{
    public abstract Vector2 Movement { get; }
    public abstract Vector2 Look { get; }
    public abstract bool Teleport { get; }
    public abstract bool TeleportUp { get; }
    public abstract bool Jump { get; }
}

using UnityEngine;

public interface IInput
{
    Vector3 direction { get; }

    Vector2 look { get; }
    bool jump { get; }
    bool dash { get; }
    bool start { get; }
}
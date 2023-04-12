using UnityEngine;

public interface IInput
{
    Vector3 direction { get; }
    bool jump { get; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovingPlatformVariable : ScriptableObject
{
    public GameObject platform;
    public float lastRotation;
    public Vector3 lastPosition;

    public void SetPlatform(GameObject platform) { this.platform = platform; }
    public void SetLastRotation(float lastRotation) { this.lastRotation = lastRotation; }
    public void SetLastPosition(Vector3 lastPosition) { this.lastPosition = lastPosition; }
}

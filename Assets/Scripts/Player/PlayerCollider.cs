using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public GameEvent reachFinish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            reachFinish.Raise();
        }
    }
}

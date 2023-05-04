using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private Vector3 start;
    private Vector3 current;
    private int destination;

    [SerializeField] private bool rotation;
    [SerializeField] private bool moveFromOrigin = true;
    [SerializeField] private bool loop = true;
    [SerializeField] private Vector3[] destiny;
    [SerializeField] private float[] timeToReachDestination;
    [SerializeField] private float[] delayAfterReachDestination;

    private void Start()
    {
        start = rotation ? transform.eulerAngles : transform.position;
        current = start;
        StartCoroutine("Move");
    }

    private IEnumerator Move()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / timeToReachDestination[destination])
        {
            if (rotation)
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(current, destiny[destination], i));
            }
            else
            {
                if (moveFromOrigin)
                {
                    transform.position = Vector3.Lerp(current, start + destiny[destination], i);
                }
                else
                {
                    transform.position = Vector3.Lerp(current, destiny[destination], i);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        if (delayAfterReachDestination.Length > destination)
        {
            yield return new WaitForSeconds(delayAfterReachDestination[destination]);
        }

        current = rotation ? transform.eulerAngles : transform.position;
        destination++;
        if (destination >= destiny.Length && loop)
        {
            destination = 0;
        }
        if (destination < destiny.Length && enabled)
        {
            StartCoroutine("Move");
        }
    }
}

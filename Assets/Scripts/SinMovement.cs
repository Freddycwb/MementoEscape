using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] private bool moveY = true;
    [SerializeField] private bool moveX;

    [SerializeField] private float frequency = 5f;
    [SerializeField] private float magnitude = 5f;
    [SerializeField] private float offset = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (moveY)
        {
            transform.position = startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
        }
        if (moveX)
        {
            transform.position = startPosition + transform.right * Mathf.Sin(Time.time * frequency + offset) * magnitude;
        }
    }
}

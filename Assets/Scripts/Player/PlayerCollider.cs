using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private Movement playerMovement;

    [SerializeField] private BoolVariable isGrounded;
    [SerializeField] private MovingPlatformVariable movingPlatform;
    private bool wasGrounded;

    private void Start()
    {
        playerMovement = GetComponent<Movement>();
        wasGrounded = isGrounded.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Thrower"))
        {
            playerMovement.BeThrown(other.gameObject.GetComponent<Thrower>().direction, other.gameObject.GetComponent<Thrower>().force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Thrower"))
        {
            playerMovement.BeThrown(collision.gameObject.GetComponent<Thrower>().direction, collision.gameObject.GetComponent<Thrower>().force);
        }
        if (collision.gameObject.CompareTag("MovingPlatform") && !wasGrounded)
        {
            movingPlatform.SetPlatform(collision.gameObject);
            movingPlatform.SetLastRotation(collision.transform.eulerAngles);
            movingPlatform.SetLastPosition(collision.transform.position);
            wasGrounded = isGrounded.Value;
        }
    }

    private void LateUpdate()
    {
        if (!isGrounded.Value)
        {
            wasGrounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (movingPlatform.platform == collision.gameObject)
        {
            movingPlatform.platform = null;
        }
    }
}
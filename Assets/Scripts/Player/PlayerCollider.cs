using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private Rigidbody rb;
    private float ySpeed;

    [SerializeField] private Vector2 ResistAndDeathVelocity;
    [SerializeField] private float hp;

    public GameEvent reachFinish;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ySpeed = rb.velocity.y != 0 ? rb.velocity.y : ySpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Exit"))
        {
            reachFinish.Raise();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            float damage = 0;
            if (ySpeed < ResistAndDeathVelocity.x)
            {
                damage = ySpeed;
            }
            Debug.Log("speed: " + ySpeed + " Damage: " + damage);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Movement movement;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Moving", Mathf.Abs(movement.rb.velocity.x) + Mathf.Abs(movement.rb.velocity.z) > 0.1f);
        animator.SetBool("IsGrounded", movement.isGrounded);
    }

    public void Finish()
    {
        animator.SetBool("Guitar", true);
    }
}

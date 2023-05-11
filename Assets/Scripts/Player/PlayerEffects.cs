using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private Animator animator;
    private IInput _input;
    [SerializeField] private BoolVariable isGrounded;
    [SerializeField] private BoolVariable isMoving;
    private bool jumpDash;
    [SerializeField] private GameObject dashParticle;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject jumpParticle;
    [SerializeField] private GameObject landParticle;
    [SerializeField] private GameObject respawnParticle;
    [SerializeField] private GameObject runParticle;
    private GameObject currentRunParticle;
    [SerializeField] private GameObject dashSound;
    [SerializeField] private GameObject deathSound;
    [SerializeField] private GameObject jumpSound;
    [SerializeField] private GameObject landSound;
    [SerializeField] private GameObject respawnSound;
    [SerializeField] private GameObject runSound;
    private GameObject currentRunSound;


    private void Start()
    {
        animator = GetComponent<Animator>();
        _input = GetComponentInParent<PlayerInput>();
    }

    private void Update()
    {
        animator.SetBool("IsGrounded", isGrounded.Value);
        animator.SetBool("IsMoving", isMoving.Value);
        if (isMoving.Value && isGrounded.Value && currentRunParticle == null)
        {
            currentRunParticle = Instantiate(runParticle, transform.position, jumpParticle.transform.rotation);
            currentRunSound = Instantiate(runSound, transform.position, jumpParticle.transform.rotation);
            currentRunParticle.transform.SetParent(transform);
            currentRunSound.transform.SetParent(transform);
        }
        else if ((!isMoving.Value && currentRunParticle != null) || !isGrounded.Value)
        {
            Destroy(currentRunParticle);
            Destroy(currentRunSound);
        }
    }

    public void Dash()
    {
        animator.SetTrigger("Dash");
        jumpDash = true;
        Vector3 vel = _input.direction.normalized;
        Vector3 heading = vel - vel.y * Vector3.up;
        float rotY = Mathf.Atan2(heading.z, -heading.x) * Mathf.Rad2Deg;
        Instantiate(dashParticle, transform.position, Quaternion.Euler(-90, rotY - 90, 0));
        Instantiate(dashSound, transform.position, transform.rotation);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
        Instantiate(deathParticle, transform.position + Vector3.up * 0.6f, deathParticle.transform.rotation);
        Instantiate(deathSound, transform.position, transform.rotation);
    }

    public void Jump()
    {
        if (jumpDash)
        {
            jumpDash = false;
            animator.SetTrigger("JumpDash");
        }
        else
        {
            animator.SetTrigger("Jump");
        }
        Instantiate(jumpParticle, transform.position, jumpParticle.transform.rotation);
        Instantiate(jumpSound, transform.position, transform.rotation);
    }

    public void Land()
    {
        if (isMoving.Value)
        {
            animator.Play("PlayerWalk");
        }
        else
        {
            animator.Play("PlayerIdle");
        }
        jumpDash = false;
        Instantiate(landParticle, transform.position, jumpParticle.transform.rotation);
        Instantiate(landSound, transform.position, transform.rotation);
    }

    public void Respawn()
    {
        animator.Play("PlayerIdle");
        Instantiate(respawnParticle, transform.position, jumpParticle.transform.rotation);
        Instantiate(respawnSound, transform.position, transform.rotation);
    }
}

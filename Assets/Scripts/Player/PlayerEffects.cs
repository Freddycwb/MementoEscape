using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerEffects : MonoBehaviour
{
    private Vector3 lastPos;
    private Animator animator;
    private IInput _input;
    private Transform myTransform;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float groundedRememberTime;
    private float groundedRemember;
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


    private void Start()
    {
        animator = GetComponent<Animator>();
        _input = GetComponentInParent<PlayerInput>();
        TryGetComponent(out myTransform);
    }

    private void Update()
    {
        if (_input.direction.magnitude != 0)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(new Vector3(-_input.direction.x, 0, -_input.direction.z)), Time.deltaTime * rotateSpeed);
        }

        groundedRemember -= Time.deltaTime;
        if (isGrounded.Value)
        {
            groundedRemember = groundedRememberTime;
        }
        animator.SetBool("IsGrounded", (groundedRemember > 0));
        animator.SetBool("IsMoving", isMoving.Value);
        if (isMoving.Value && isGrounded.Value && currentRunParticle == null)
        {
            currentRunParticle = Instantiate(runParticle, transform.position, jumpParticle.transform.rotation);
            currentRunParticle.transform.SetParent(transform);
        }
        else if ((!isMoving.Value && currentRunParticle != null) || !isGrounded.Value)
        {
            Destroy(currentRunParticle);
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

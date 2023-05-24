using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    private IInput _input;

    [SerializeField] private BoolVariable isGrounded;
    [SerializeField] private BoolVariable isMoving;
    [SerializeField] private MovingPlatformVariable movingPlatform;
    [SerializeField] private GameEvent dash;
    [SerializeField] private GameEvent jump;
    [SerializeField] private GameEvent land;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAccel;

    [SerializeField] private float gravityScale;
    private float globalGravity = -9.81f;

    [SerializeField] private float holdJumpTime;
    private float holdJump;
    [SerializeField] private float jumpPressedRememberTime;
    private float jumpPressedRemember;
    [SerializeField] private float groundedRememberTime;
    private float groundedRemember;
    private bool justJumped;
    [SerializeField] private float jumpForce;
    private bool canDoubleJump;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Vector2 dashForce;
    [SerializeField] private float dashJumpForce;
    private bool canDash;
    private bool canDashJump;
    private bool dashJumping;
    
    [SerializeField] private bool cameraYRotate;

    private void Awake()
    {
        movingPlatform.SetPlatform(null);
    }

    void Start()
    {
        _input = GetComponent<IInput>();
    }

    void FixedUpdate()
    {
        HorizontalMove();
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void Update()
    {
        FollowPlatform();
        Jump();
        Dash();
    }

    private void FollowPlatform()
    {
        if (movingPlatform.platform == null)
        {
            return;
        }

        // Follow moving platform
        Vector3 moveDistance = movingPlatform.platform.transform.position - movingPlatform.lastPosition;
        transform.position += moveDistance;

        // Follow rotating platform
        float rot, rotRad;
        Vector3 a, b, diff;

        // y rotation
        rot = movingPlatform.platform.transform.eulerAngles.y - movingPlatform.lastRotation.y;
        rotRad = rot * Mathf.Deg2Rad;
        a = new Vector3(Mathf.Sin(rotRad), 0, Mathf.Cos(rotRad));
        b = new Vector3(Mathf.Cos(rotRad), 0, -Mathf.Sin(rotRad));
        diff = transform.position - movingPlatform.platform.transform.position;
        diff.y = 0;
        transform.position += diff.x * b + diff.z * a - diff;
        if (cameraYRotate) transform.Rotate(0, rot, 0);

        movingPlatform.SetLastRotation(movingPlatform.platform.transform.eulerAngles);
        movingPlatform.SetLastPosition(movingPlatform.platform.transform.position);
    }

    void HorizontalMove()
    {
        Vector3 goalVel = _input.direction.normalized * maxSpeed;   
        Vector3 neededAccel = goalVel - rb.velocity;
        neededAccel -= Vector3.up * neededAccel.y;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        rb.AddForce(neededAccel, ForceMode.Impulse);
        isMoving.Value = Mathf.Abs(rb.velocity.x + rb.velocity.z) > 0.1f;
    }

    void Jump()
    {
        Collider[] grounds = Physics.OverlapSphere(transform.position, groundCheckRadius, whatIsGround);
        if (!isGrounded.Value && grounds.Length > 0)
        {
            land.Raise();
        }
        isGrounded.Value = grounds.Length > 0;
        groundedRemember -= Time.deltaTime;
        if (isGrounded.Value && !justJumped)
        {
            groundedRemember = groundedRememberTime;
            canDoubleJump = true;
            canDash = true;
            canDashJump = false;
            dashJumping = false;
        }

        jumpPressedRemember -= Time.deltaTime;
        if (_input.jump)
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }
        else
        {
            justJumped = false;
        }
        if (!_input.jump && isGrounded.Value)
        {
            jumpPressedRemember = 0;
        }
        if ((_input.jump && justJumped) || (((jumpPressedRemember > 0) && (groundedRemember > 0)) || (_input.jump && !(groundedRemember > 0) && canDoubleJump) || (_input.jump && !(groundedRemember > 0) && canDashJump)))
        {
            if (!justJumped)
            {
                jump.Raise();
                justJumped = true;
                if (!(groundedRemember > 0) && canDoubleJump)
                {
                    canDoubleJump = false;
                }
                if (!canDashJump)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x, dashJumpForce, rb.velocity.z);
                    canDashJump = false;
                    dashJumping = true;
                }
                holdJump = holdJumpTime;
                jumpPressedRemember = 0;
                groundedRemember = 0;
            }
            else
            {
                if (holdJump > 0)
                {
                    if (!dashJumping)
                    {
                        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    }
                    else
                    {
                        rb.velocity = new Vector3(rb.velocity.x, dashJumpForce, rb.velocity.z);
                    }
                    holdJump -= Time.deltaTime;
                }
            }
        }
    }

    void Dash()
    {
        if (_input.dash && canDash && _input.direction != Vector3.zero)
        {
            dash.Raise();
            canDash = false;
            canDoubleJump = false;
            canDashJump = true;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(new Vector3(_input.direction.normalized.x * dashForce.x, dashForce.y, _input.direction.normalized.z * dashForce.x), ForceMode.Impulse);
        }
    }

    public void BeThrown(Vector3 dir, float force)
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}

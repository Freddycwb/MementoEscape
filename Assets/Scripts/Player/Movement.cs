using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    private IInput _input;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAccel;

    [SerializeField] private float jumpForce;
    public bool isGrounded;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    void Start()
    {
        _input = GetComponent<IInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        HorizontalMove();
    }

    private void Update()
    {
        Jump();
    }

    void HorizontalMove()
    {
        Vector3 goalVel = _input.direction.normalized * maxSpeed;   
        Vector3 neededAccel = goalVel - rb.velocity;
        neededAccel -= Vector3.up * neededAccel.y;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        rb.AddForce(neededAccel, ForceMode.Impulse);
    }

    void Jump()
    {
        isGrounded = Physics.OverlapSphere(transform.position, groundCheckRadius, whatIsGround).Length > 0;
        if (_input.jump && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }
}

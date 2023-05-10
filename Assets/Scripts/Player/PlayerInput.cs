using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IInput
{
    public GameObject camera;
    private bool canControl = true;

    [SerializeField] private InputActionAsset playerControls;
    private InputActionMap controls;
    private bool controlJump;
    private bool controlDash;


    private void Awake()
    {
        controls = playerControls.FindActionMap("Gameplay");
        controls.FindAction("Jump").performed += ctx => Jump();
        controls.FindAction("Dash").performed += ctx => Dash();
    }

    private void Jump()
    {
        controlJump = true;
    }

    private void Dash()
    {
        controlDash = true;
    }

    public Vector3 direction
    {
        get
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            Vector3 move = new Vector3(x, 0, z);

            float headAngle = Mathf.Deg2Rad * (360 - camera.transform.rotation.eulerAngles.y);

            Vector3 a = new Vector3(Mathf.Cos(headAngle), 0, Mathf.Sin(headAngle));
            Vector3 b = new Vector3(-Mathf.Sin(headAngle), 0, Mathf.Cos(headAngle));

            Vector3 rotatedMove = move.x * a + move.z * b;

            if (canControl)
            {
                return rotatedMove;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    public bool jump
    {
        get
        {
            if (canControl)
            {
                return Input.GetKeyDown(KeyCode.Space) || controlJump;
            }
            else
            {
                return false;
            }
        }
    }

    public bool dash
    {
        get
        {
            if (canControl)
            {
                return Input.GetKeyDown(KeyCode.LeftShift) || controlDash;
            }
            else
            {
                return false;
            }
        }
    }

    private void LateUpdate()
    {
        controlJump = false;
        controlDash = false;
    }

    public void SetCanControl(bool state)
    {
        canControl = state;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap("Gameplay").Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap("Gameplay").Disable();
    }
}

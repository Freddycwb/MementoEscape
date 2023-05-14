using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour, IInput
{
    [SerializeField] private GameObject camera;
    private bool canControl = true;


    public Vector3 direction
    {
        get
        {
            Vector3 gamepadMove = Vector3.zero;
            if (Gamepad.current != null)
            {
                StickControl stick = Gamepad.current.leftStick;
                gamepadMove = new Vector3(stick.right.value - stick.left.value, 0, stick.up.value - stick.down.value);
            }
            Vector3 keyboardMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 move = keyboardMove + gamepadMove;

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

    public Vector2 look
    {
        get
        {
            if (!canControl)
            {
                return Vector2.zero;
            }
            Vector2 gamepadLook = Vector2.zero;
            if (Gamepad.current != null)
            {
                StickControl stick = Gamepad.current.rightStick;
                gamepadLook = new Vector2(stick.up.value - stick.down.value, stick.right.value - stick.left.value);
            }
            Vector2 mouseLook = new Vector2(Mouse.current.delta.value.y, Mouse.current.delta.value.x);
            return mouseLook + gamepadLook;
        }
    }

    public bool jump
    {
        get
        {
            if (canControl)
            {
                bool gamepadJump = false;
                if (Gamepad.current != null)
                {
                    gamepadJump = Gamepad.current.buttonSouth.wasPressedThisFrame;
                }
                return Input.GetKeyDown(KeyCode.Space) || gamepadJump;
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
                bool gamepadDash = false;
                if (Gamepad.current != null)
                {
                    gamepadDash = Gamepad.current.rightTrigger.wasPressedThisFrame;
                }
                return Input.GetKeyDown(KeyCode.LeftShift) || gamepadDash;
            }
            else
            {
                return false;
            }
        }
    }

    public void SetCanControl(bool state)
    {
        canControl = state;
    }
}

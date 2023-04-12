using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IInput
{
    public GameObject camera;

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

            return rotatedMove;
        }
    }

    public bool jump
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }
    }
}

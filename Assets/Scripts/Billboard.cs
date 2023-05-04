using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool moveY;
    Vector3 cameraDir;
    [SerializeField] private float rotateVel;
    private Transform myTransform;
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.GetComponent<Transform>();
        TryGetComponent(out myTransform);
    }
    private void Update()
    {
        cameraDir = cam.forward;
        if (moveY)
        {
            cameraDir.y = 0;
        }

        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(cameraDir), Time.deltaTime * rotateVel);
    }
}

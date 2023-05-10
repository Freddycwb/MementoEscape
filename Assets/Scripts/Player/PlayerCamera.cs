using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineFreeLook cameraSetting;

    private void Start()
    {
        cameraSetting = GetComponent<CinemachineFreeLook>();
        SetSensitivity();
    }

    public void SetSensitivity()
    {
        cameraSetting.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sensitivity") * 3000;
        cameraSetting.m_YAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sensitivity") * 20;
    }
}

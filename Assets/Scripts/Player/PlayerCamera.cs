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
    }

    public void SetSensitivity()
    {
        cameraSetting.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sensitivity") * 1500;
        cameraSetting.m_YAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sensitivity") * 10;
    }
}

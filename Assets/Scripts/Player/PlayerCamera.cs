using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineFreeLook cameraSetting;
    private IInput _input;
    [SerializeField] private GameObject player;
    [SerializeField] private float senseX = 750;
    [SerializeField] private float senseY = 5;

    private void Start()
    {
        cameraSetting = GetComponent<CinemachineFreeLook>();
        _input = player.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        cameraSetting.m_XAxis.Value += _input.look.y * PlayerPrefs.GetFloat("sensitivity") * senseX * Time.deltaTime;
        cameraSetting.m_YAxis.Value += -_input.look.x * PlayerPrefs.GetFloat("sensitivity") * senseY * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineFreeLook cameraSetting;
    private IInput _input;
    [SerializeField] private GameObject player;

    private void Start()
    {
        cameraSetting = GetComponent<CinemachineFreeLook>();
        _input = player.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        cameraSetting.m_XAxis.Value += _input.look.y * PlayerPrefs.GetFloat("sensitivity") * 750 * Time.deltaTime;
        cameraSetting.m_YAxis.Value += -_input.look.x * PlayerPrefs.GetFloat("sensitivity") * 5 * Time.deltaTime;
    }
}

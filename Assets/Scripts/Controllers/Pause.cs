using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;

    private void Awake()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
        sensitivitySlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        }));
    }
}

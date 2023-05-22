using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Awake()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
        sensitivitySlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
        }));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        musicVolumeSlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        }));
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        sfxVolumeSlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
        }));
    }
}

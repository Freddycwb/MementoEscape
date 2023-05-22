using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] audios;
    [SerializeField] private bool music;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = music ? PlayerPrefs.GetFloat("musicVolume") : PlayerPrefs.GetFloat("sfxVolume");
        source.clip = audios[Random.Range(0, audios.Length)];
        source.enabled = true;
    }

    private void Update()
    {
        if (music) source.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}

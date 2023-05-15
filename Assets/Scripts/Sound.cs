using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip[] audios;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("sfxVolume");
        source.clip = audios[Random.Range(0, audios.Length)];
    }
}

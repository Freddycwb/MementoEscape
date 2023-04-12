using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;

    public float timeToFinish;

    private void Start()
    {
        PlayerPrefs.SetFloat("gameTime", timeToFinish);
    }

    private void Update()
    {
        if (timeToFinish <= 0)
        {
            PlayerPrefs.SetFloat("currentTime", -1);
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            timer.text = (Mathf.Floor(timeToFinish / 60)).ToString() + ":" + ((int)timeToFinish % 60).ToString("d2");
            timeToFinish -= Time.deltaTime;
        }
    }

    public void Victory()
    {
        PlayerPrefs.SetFloat("currentTime", timeToFinish);
        SceneManager.LoadScene("GameOver");
    }
}

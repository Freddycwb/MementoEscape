using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;
using Cinemachine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject thirdPersonCamera;
    public int lifes;
    private int maxLifes;
    [SerializeField] private GameEvent respawn;
    private Vector3 spawnPoint;

    [SerializeField] private GameObject hud;
    [SerializeField] private Image status;
    [SerializeField] private Sprite[] statusHP;
    private Animator hudAnimator;
    [SerializeField] private TextMeshProUGUI timerTMP;
    public float timeToFinish;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    private float scoreValue;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private GameEvent sensitivityChange;

    private void Awake()
    {
        sensitivitySlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
            sensitivityChange.Raise();
        }));
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetFloat("gameTime", timeToFinish);

        hudAnimator = hud.GetComponent<Animator>();

        maxLifes = lifes;
        spawnPoint = player.transform.position;
    }

    private void Update()
    {
        Pause();
        Timer();
        Score();
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            player.GetComponent<PlayerInput>().SetCanControl(!pauseMenu.activeSelf);
            thirdPersonCamera.GetComponentInChildren<CinemachineFreeLook>().enabled = !pauseMenu.activeSelf;
            if (pauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void Timer()
    {
        if (timeToFinish <= 0)
        {
            PlayerPrefs.SetFloat("currentTime", -1);
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            timerTMP.text = (Mathf.Floor(timeToFinish / 60)).ToString() + ":" + ((int)timeToFinish % 60).ToString("d2");
            timeToFinish -= Time.deltaTime;
        }
    }

    private void Score()
    {
        scoreTMP.text = scoreValue.ToString();
    }

    public void Death()
    {
        lifes--;
        if (lifes <= 0)
        {
            PlayerPrefs.SetFloat("currentTime", -10);
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            StartCoroutine("Respawn");
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.75f);
        player.transform.position = spawnPoint;
        hudAnimator.Play("ShowStatus");
        yield return new WaitForSeconds(0.25f);
        status.sprite = statusHP[lifes];
        respawn.Raise();
    }

    public void Checkpoint()
    {
        spawnPoint = player.transform.position;
    }

    public void PickedLife()
    {
        if (lifes < maxLifes)
        {
            lifes++;
        }
        StartCoroutine("ShowStatus");
    }

    public void PickedPoint()
    {
        scoreValue += 10;
    }

    private IEnumerator ShowStatus()
    {
        hudAnimator.Play("ShowStatus");
        yield return new WaitForSeconds(0.25f);
        status.sprite = statusHP[lifes];
    }

    public void Victory()
    {
        PlayerPrefs.SetFloat("currentTime", timeToFinish);
        SceneManager.LoadScene("GameOver");
    }
}

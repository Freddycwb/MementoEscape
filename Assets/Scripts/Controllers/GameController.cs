using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Cinemachine;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool test;
    [SerializeField] private StringVariable cameFrom;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private FloatVariable lastRunTime;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private CanvasObjects canvas;
    [SerializeField] private GameEvent respawn;
    private Vector3 spawnPoint;

    public float timeToFinish;
    [SerializeField] private FloatVariable score;

    [SerializeField] private GameObject tutorial;
    [SerializeField] private Vector3 tutorialPlayerPos;
    [SerializeField] private GameObject store;
    [SerializeField] private Vector3 storePlayerPos;
    [SerializeField] private Animator camAnim;

    [SerializeField] private bool startTimer;
    [SerializeField] private Vector3[] tps;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameTime.Value = timeToFinish;
        score.Value = 0;

        if (!test)
        {
            canvas.hud.SetActive(false);
            if (cameFrom.Value == "CutsceneTutorial" || cameFrom.Value == "")
            {
                Instantiate(tutorial);
                player.transform.position = tutorialPlayerPos;
                camAnim.Play("ShowTutorialExit");
                StartCoroutine("EnablePlayer", 5);
            }
            else
            {
                Instantiate(store);
                player.transform.position = storePlayerPos;
                camAnim.Play("ShowStoreExit");
                StartCoroutine("EnablePlayer", 12);
            }
            player.GetComponent<PlayerInput>().SetCanControl(false);
        }
        else
        {
            startTimer = true;
        }

        spawnPoint = player.transform.position;
    }

    private IEnumerator EnablePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        player.GetComponent<PlayerInput>().SetCanControl(true);
        if (cameFrom.Value != "CutsceneTutorial" && cameFrom.Value != "")
        {
            canvas.hud.SetActive(true);
            startTimer = true;
        }
    }

    private void Update()
    {
        Pause();
        if (startTimer)
        {
            Timer();
        }
        Score();
        Cheat();
    }

    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            player.transform.position = tps[0];
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.transform.position = tps[1];
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.transform.position = tps[2];
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            player.transform.position = tps[3];
        }
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            canvas.pauseMenu.SetActive(!canvas.pauseMenu.activeSelf);
            player.GetComponent<PlayerInput>().SetCanControl(!canvas.pauseMenu.activeSelf);
            thirdPersonCamera.GetComponentInChildren<CinemachineFreeLook>().enabled = !canvas.pauseMenu.activeSelf;
            if (canvas.pauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Time.timeScale = !canvas.pauseMenu.activeSelf ? 1f : 0f;
        }
    }

    private void Timer()
    {
        if (timeToFinish <= 0)
        {
            cameFrom.Value = "GameTimerRunOut";
            SceneManager.LoadScene("Menu");
        }
        else
        {
            canvas.timerSlider.value = timeToFinish / gameTime.Value;
            canvas.timerTMP.text = (Mathf.Floor(timeToFinish / 60)).ToString() + ":" + ((int)timeToFinish % 60).ToString("d2");
            timeToFinish -= Time.deltaTime;
        }
    }

    private void Score()
    {
        canvas.scoreTMP.text = score.Value.ToString();
    }

    public void Death()
    {
        StartCoroutine("Respawn");
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.75f);
        player.transform.position = spawnPoint;
        yield return new WaitForSeconds(0.25f);
        respawn.Raise();
    }

    public void Checkpoint()
    {
        spawnPoint = player.transform.position;
    }

    public void PickedLife()
    {
        timeToFinish += 10;
    }

    public void PickedPoint()
    {
        score.Value += 100;
    }

    public void Finish()
    {
        if (cameFrom.Value == "CutsceneTutorial" || cameFrom.Value == "")
        {
            cameFrom.Value = "GameTutorialEnd";
            StartCoroutine("CallTransition", "Cutscene");
        }
        else
        {
            cameFrom.Value = "GameVictory";
            lastRunTime.Value = timeToFinish;
            StartCoroutine("CallTransition", "Menu");
        }
    }

    private IEnumerator CallTransition(string scene)
    {
        canvas.blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}

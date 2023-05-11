using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Cinemachine;

public class GameController : MonoBehaviour
{
    [SerializeField] private StringVariable cameFrom;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private FloatVariable lastRunTime;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private CanvasObjects canvas;
    [SerializeField] private GameEvent respawn;
    private Vector3 spawnPoint;

    public float timeToFinish;
    private float scoreValue;

    [SerializeField] private GameObject tutorial;
    [SerializeField] private Vector3 tutorialPlayerPos;
    [SerializeField] private GameObject store;
    [SerializeField] private Vector3 storePlayerPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameTime.Value = timeToFinish;

        if (cameFrom.Value == "CutsceneTutorial" || cameFrom.Value == "")
        {
            Instantiate(tutorial);
            player.transform.position = tutorialPlayerPos;
            canvas.hud.SetActive(false);
        }
        else if (cameFrom.Value == "CutsceneSkipTutorial" || cameFrom.Value == "CutsceneTutorialEnd")
        {
            Instantiate(store);
            player.transform.position = storePlayerPos;
        }
        spawnPoint = player.transform.position;
    }

    private void Update()
    {
        Pause();
        if (cameFrom.Value == "CutsceneSkipTutorial" || cameFrom.Value == "CutsceneTutorialEnd")
        {
            Timer();
        }
        Score();
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
            canvas.timerTMP.text = (Mathf.Floor(timeToFinish / 60)).ToString() + ":" + ((int)timeToFinish % 60).ToString("d2");
            timeToFinish -= Time.deltaTime;
        }
    }

    private void Score()
    {
        canvas.scoreTMP.text = scoreValue.ToString();
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
        scoreValue += 10;
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

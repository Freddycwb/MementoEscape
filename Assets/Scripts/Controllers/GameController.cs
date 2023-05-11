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

    [SerializeField] private GameObjectVariable player;
    [SerializeField] private GameObject thirdPersonCamera;
    [SerializeField] private GameEvent respawn;
    private Vector3 spawnPoint;

    [SerializeField] private GameObject hud;
    [SerializeField] private TextMeshProUGUI timerTMP;
    public float timeToFinish;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    private float scoreValue;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator blackscreen;

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
            player.Value.transform.position = tutorialPlayerPos;
            hud.SetActive(false);
        }
        else if (cameFrom.Value == "CutsceneSkipTutorial" || cameFrom.Value == "CutsceneTutorialEnd")
        {
            Instantiate(store);
            player.Value.transform.position = storePlayerPos;
        }
        spawnPoint = player.Value.transform.position;
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
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            player.Value.GetComponent<PlayerInput>().SetCanControl(!pauseMenu.activeSelf);
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
            cameFrom.Value = "GameTimerRunOut";
            SceneManager.LoadScene("Menu");
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
        StartCoroutine("Respawn");
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.75f);
        player.Value.transform.position = spawnPoint;
        yield return new WaitForSeconds(0.25f);
        respawn.Raise();
    }

    public void Checkpoint()
    {
        spawnPoint = player.Value.transform.position;
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
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}

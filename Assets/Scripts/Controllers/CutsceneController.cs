using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private StringVariable cameFrom;

    private Animator animator;
    [SerializeField] private Animator blackscreen;
    private string playingCutscene;
    private float skipCutsceneCount;
    private bool gamepadActionDone;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (cameFrom.Value == "Menu")
        {
            playingCutscene = "TutorialOption";
        }
        else if (cameFrom.Value == "GameTutorialEnd")
        {
            Cursor.lockState = CursorLockMode.Locked;
            playingCutscene = "TutorialEnd";
            animator.Play("TutorialEndCutscene");
            StartCoroutine("WaitForTutorialEndCutscene");
            cameFrom.Value = "CutsceneTutorialEnd";
        }
    }

    private void Update()
    {
        if (!gamepadActionDone && cameFrom.Value != "CutsceneTutorialEnd" && Gamepad.current != null)
        {
            GamepadControls();
        }
        HoldAnyButton();
    }

    private void GamepadControls()
    {
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            PlayTutorial();
            gamepadActionDone = true;
        }
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            SkipTutorial();
            gamepadActionDone = true;
        }
    }

    private void HoldAnyButton()
    {
        if (Input.anyKey && playingCutscene != "TutorialOption")
        {
            skipCutsceneCount += Time.deltaTime;
            if (skipCutsceneCount > 1)
            {
                StartCoroutine("SkipCutscene", "Game");
            }
        }
        else
        {
            skipCutsceneCount = 0;
        }
    }

    public void PlayTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playingCutscene = "TutorialCutscene";
        animator.Play("TutorialCutscene");
        blackscreen.Play("Transition");
        StartCoroutine("WaitForTutorialCutscene");
        cameFrom.Value = "CutsceneTutorial";
    }

    public void SkipTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playingCutscene = "FullCutscene";
        animator.Play("FullCutscene");
        blackscreen.Play("Transition");
        StartCoroutine("WaitForFullCutscene");
        cameFrom.Value = "CutsceneSkipTutorial";
    }

    public IEnumerator WaitForTutorialCutscene()
    {
        yield return new WaitForSeconds(11);
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator WaitForTutorialEndCutscene()
    {
        yield return new WaitForSeconds(6);
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator WaitForFullCutscene()
    {
        yield return new WaitForSeconds(17);
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    private IEnumerator SkipCutscene(string scene)
    {
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}

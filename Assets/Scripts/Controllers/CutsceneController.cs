using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    private IInput _input;
    [SerializeField] private StringVariable cameFrom;

    private Animator animator;
    [SerializeField] private Animator blackscreen;
    private string playingCutscene;
    private bool gamepadActionDone;

    private void Start()
    {
        _input = GetComponent<IInput>();
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
        if (_input.start && playingCutscene != "TutorialOption")
        {
            StartCoroutine("SkipCutscene", "Game");
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
        yield return new WaitForSeconds(21);
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator WaitForTutorialEndCutscene()
    {
        yield return new WaitForSeconds(15);
        blackscreen.Play("Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator WaitForFullCutscene()
    {
        yield return new WaitForSeconds(36);
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

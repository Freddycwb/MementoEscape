using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Animator blackscreen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator.Play("TutorialCutscene");
        blackscreen.Play("Transition");
        StartCoroutine("WaitForTutorialCutscene");
    }

    public void SkipTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator.Play("FullCutscene");
        blackscreen.Play("Transition");
        StartCoroutine("WaitForFullCutscene");
    }

    public IEnumerator WaitForTutorialCutscene()
    {
        yield return new WaitForSeconds(11);
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
}

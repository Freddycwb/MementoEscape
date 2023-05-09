using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void PlayTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator.Play("TutorialCutscene");
        StartCoroutine("WaitForTutorialCutscene");
    }

    public void SkipTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator.Play("FullCutscene");
        StartCoroutine("WaitForFullCutscene");
    }

    public IEnumerator WaitForTutorialCutscene()
    {
        yield return new WaitForSeconds(12);
        SceneManager.LoadScene("Game");
    }

    public IEnumerator WaitForFullCutscene()
    {
        yield return new WaitForSeconds(17.5f);
        SceneManager.LoadScene("Game");
    }
}

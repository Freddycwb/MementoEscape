using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Sprite timeOverBackground;

    private Animator animator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.GetFloat("currentTime") == -1)
        {
            background.sprite = timeOverBackground;
        }
        else
        {
            float t = Mathf.Abs(PlayerPrefs.GetFloat("currentTime") - PlayerPrefs.GetFloat("gameTime"));
            time.text = (Mathf.Floor(t / 60)).ToString() + ":" + ((int)t % 60).ToString("d2"); ;
        }
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        PlayerPrefs.SetFloat("cutscene", 0);
        animator.enabled = true;
        StartCoroutine("WaitForTransition");
    }

    public IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Cutscene");
    }
}

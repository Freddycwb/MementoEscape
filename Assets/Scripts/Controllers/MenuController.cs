using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private StringVariable cameFrom;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private FloatVariable lastRunTime;

    [SerializeField] private Image background;

    [SerializeField] private GameObject main;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Animator blackscreen;

    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Sprite timeOverBackground;
    [SerializeField] private Sprite victoryBackground;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (cameFrom.Value == "GameTimerRunOut")
        {
            background.sprite = timeOverBackground;
            main.SetActive(false);
            gameOver.SetActive(true);  
            cameFrom.Value = "MenuTimerRunOut";
        }
        else if (cameFrom.Value == "GameVictory")
        {
            background.sprite = victoryBackground;
            main.SetActive(false);
            gameOver.SetActive(true);
            float t = Mathf.Abs(lastRunTime.Value - gameTime.Value);
            time.text = (Mathf.Floor(t / 60)).ToString() + ":" + ((int)t % 60).ToString("d2");
            cameFrom.Value = "MenuTimerRunOut";
        }
        if (cameFrom.Value == "")
        {
            cameFrom.Value = "Menu";
        }
    }

    public void Play()
    {
        blackscreen.Play("Transition");
        StartCoroutine("WaitForTransition");
    }

    public IEnumerator WaitForTransition()
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene("Cutscene");
    }
}

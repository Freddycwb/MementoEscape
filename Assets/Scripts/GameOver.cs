using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI time;

    [SerializeField] private Sprite timeOverBackground;
    [SerializeField] private Sprite deathBackground;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (PlayerPrefs.GetFloat("currentTime") == -1)
        {
            background.sprite = timeOverBackground;
        }
        else if (PlayerPrefs.GetFloat("currentTime") == -10)
        {
            background.sprite = deathBackground;
        }
        else
        {
            float t = Mathf.Abs(PlayerPrefs.GetFloat("currentTime") - PlayerPrefs.GetFloat("gameTime"));
            time.text = (Mathf.Floor(t / 60)).ToString() + ":" + ((int)t % 60).ToString("d2"); ;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    [SerializeField] private StringVariable cameFrom;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private FloatVariable lastRunTime;
    [SerializeField] private FloatVariable lastRunScore;

    [SerializeField] private Image background;

    [SerializeField] private GameObject main;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject victory;
    [SerializeField] private Animator blackscreen;

    [SerializeField] private Sprite mainBackground;
    [SerializeField] private Sprite gameoverBackground;
    [SerializeField] private Sprite victoryBackground;

    private bool showingLeaderboard;
    [SerializeField] private ListStringVariable names;
    [SerializeField] private ListFloatVariable scores;
    [SerializeField] private ListFloatVariable times;
    [SerializeField] private FloatVariable record;
    [SerializeField] private GameObject lLine;

    [SerializeField] private GameObject recordSound;
    [SerializeField] private TextMeshProUGUI gradeTMP;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private GameObject scoreSound;
    [SerializeField] private GameObject gradeSound;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private float[] gradeScores;
    [SerializeField] private string[] gradeTitles;
    [SerializeField] private float scrollSpeed;
    private bool scrollingDown;
    private bool scrollCanGo = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (cameFrom.Value == "GameTimerRunOut")
        {
            background.sprite = gameoverBackground;
            main.SetActive(false);
            gameOver.SetActive(true);  
            cameFrom.Value = "MenuTimerRunOut";
        }
        else if (cameFrom.Value == "GameVictory")
        {
            background.sprite = victoryBackground;
            main.SetActive(false);
            victory.SetActive(true);
            StartCoroutine("IncreaseScore");
            cameFrom.Value = "MenuVictory";
        }
        else
        {
            background.sprite = mainBackground;
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

    public void SaveName()
    {
        int index = names.Value.IndexOf(nameField.text);
        if (index != -1)
        {
            scores.Value.Remove(scores.Value[index]);
            names.Value.Remove(nameField.text);
            times.Value.Remove(times.Value[index]);
        }
        if (nameField.text != "")
        {
            names.Value.Add(nameField.text);
            scores.Value.Add(lastRunTime.Value * 100 + lastRunScore.Value);
            times.Value.Add(lastRunTime.Value);
            record.Value = lastRunTime.Value * 100 + lastRunScore.Value;
        }
        OrganizeLeaderboard();
        CreateLeaderboard();
    }

    private void OrganizeLeaderboard()
    {
        List<float> disorganizedScores = new List<float>(scores.Value);
        List<string> disorganizedNames = new List<string>(names.Value);
        List<float> newScores = new List<float>();
        List<string> newNames = new List<string>();
        List<float> newTimes = new List<float>();
        List<float> organizedScores = new List<float>(disorganizedScores);

        organizedScores.Sort();
        for (int i = organizedScores.Count - 1; i >= 0; i--)
        {
            newScores.Add(organizedScores[i]);
        }

        for (int i = 0; i < disorganizedNames.Count; i++)
        {
            newNames.Add(names.Value[disorganizedScores.IndexOf(newScores[i])]);
            newTimes.Add(times.Value[disorganizedScores.IndexOf(newScores[i])]);
        }
        scores.Value = newScores;
        names.Value = newNames;
        times.Value = newTimes;
    }

    private void CreateLeaderboard()
    {
        LeaderboardLine lastLine = null;
        for (int i = 0; i < scores.Value.Count; i++)
        {
            LeaderboardLine l = Instantiate(lLine).GetComponent<LeaderboardLine>();
            l.gameObject.transform.SetParent(scrollViewContent.transform);
            l.rank.text = i + 1 + "º";
            l.name.text = names.Value[i];
            l.score.text = scores.Value[i].ToString("0");
            float t = gameTime.Value - times.Value[i];
            l.time.text = (Mathf.Floor(t / 60)).ToString() + ":" + ((int)t % 60).ToString("d2");
            if (l.name.text == nameField.text)
            {
                l.PaintLine(Color.black);
            }
            if (i == 0)
            {
                l.PaintLine(new Color(255, 255, 0));
            }
            else if (i == 1)
            {
                l.PaintLine(Color.red);
            }
            else if (i == 2)
            {
                l.PaintLine(Color.blue);
            }
            if (lastLine == null)
            {
                l.gameObject.transform.localPosition = Vector3.zero ;
            }
            else
            {
                l.gameObject.transform.localPosition = lastLine.gameObject.transform.localPosition + Vector3.down * 30;
            }
            lastLine = l;
        }
    }

    private void Update()
    {
        if (scores.Value.Count > 10 && showingLeaderboard)
        {
            AutoScroll();
        }
    }

    public void ShowLeaderboard()
    {
        showingLeaderboard = true;
    }

    private IEnumerator IncreaseScore()
    {
        yield return new WaitForSeconds(5.5f);
        float soundTime = 1250;
        for (int i = 0; i < lastRunTime.Value * 100 + lastRunScore.Value; i += 125)
        {
            scoreTMP.text = i.ToString();
            soundTime -= i;
            if (soundTime <= 0)
            {
                soundTime = 1250;
                Instantiate(scoreSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
        scoreTMP.text = (lastRunTime.Value * 100 + lastRunScore.Value).ToString("0");
        yield return new WaitForSeconds(1.5f);
        gradeTMP.text = GetGrade();
        float t = gameTime.Value - lastRunTime.Value;
        timeTMP.text = (Mathf.Floor(t / 60)).ToString() + ":" + ((int)t % 60).ToString("d2");
        gradeSound.SetActive(true);
        yield return new WaitForSeconds(1);
        if (record.Value < lastRunTime.Value * 100 + lastRunScore.Value)
        {
            recordSound.SetActive(true);
            yield return new WaitForSeconds(2);
        }
        nameField.gameObject.transform.parent.gameObject.SetActive(true);
    }

    private string GetGrade()
    {
        string grade = "Eerrr";
        for (int i = 0; i < gradeTitles.Length; i++)
        {
            if (gradeScores[i] < lastRunTime.Value * 100 + lastRunScore.Value)
            {
                grade = gradeTitles[i];
                break;
            }
        }
        return grade;
    }

    private void AutoScroll()
    {
        if (!scrollCanGo) return;
        if (!scrollingDown)
        {
            if (scrollViewContent.transform.localPosition.y <= 0)
            {
                StartCoroutine("ChangeScrollDirection");
                scrollCanGo = false;
            }
            else
            {
                scrollViewContent.transform.Translate(0, -Time.deltaTime * scrollSpeed, 0);
            }
        }
        else
        {
            if (scrollViewContent.transform.localPosition.y >= 30 * (scores.Value.Count - 10))
            {
                StartCoroutine("ChangeScrollDirection");
                scrollCanGo = false;
            }
            else
            {
                scrollViewContent.transform.Translate(0, Time.deltaTime * scrollSpeed, 0);
            }
        }
    }

    private IEnumerator ChangeScrollDirection()
    {
        yield return new WaitForSeconds(3);
        scrollingDown = !scrollingDown;
        scrollCanGo = true;
    }
}

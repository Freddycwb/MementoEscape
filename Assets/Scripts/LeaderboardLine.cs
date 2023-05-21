using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardLine : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI name;
    public TextMeshProUGUI score;
    public TextMeshProUGUI time;

    public void PaintLine(Color c)
    {
        rank.color = c;
        score.color = c;
        name.color = c;
        time.color = c;
    }
}

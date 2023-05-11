using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasObjects : MonoBehaviour
{
    [SerializeField] private GameObjectVariable canvas;
    public GameObject hud;
    public TextMeshProUGUI timerTMP;
    public TextMeshProUGUI scoreTMP;
    public GameObject pauseMenu;
    public Animator blackscreen;

    private void Awake()
    {
        canvas.Value = gameObject;
    }
}

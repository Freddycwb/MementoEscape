using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private GameEvent sensitivityChange;

    private void Awake()
    {
        sensitivitySlider.onValueChanged.AddListener((delegate {
            PlayerPrefs.SetFloat("sensitivity", sensitivitySlider.value);
            sensitivityChange.Raise();
        }));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

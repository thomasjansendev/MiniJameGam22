using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TimerUIHandler : MonoBehaviour
{
    private Timer _timer;
    private TextMeshProUGUI _guiText;
    private int _time;
    
    private void Start()     
    {
        _guiText = GetComponent<TextMeshProUGUI>();
        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }
    
    public void Update()
    {
        _guiText.text = _timer.TimeRemaining.ToString("#");

        if (_timer.TimeRemaining <= 30f)
            _guiText.color = Color.red;


    }
        
}

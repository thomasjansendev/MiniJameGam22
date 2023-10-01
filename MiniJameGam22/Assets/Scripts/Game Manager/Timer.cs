using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    //[SerializeField] private float cutoffTimeInMinutes = 0.1f;
    [SerializeField] private float cutoffTimeInSeconds = 10f;
    private float _startTime;
    private float _elapsedTime;
    private bool _isTimerOn = false;

    public bool IsGameOver { get; private set; }
    public float TimeRemaining { get; private set; }
    
    private void Start()
    {
        IsGameOver = false;
    }

    private void Update()
    {
        if(IsGameOver)
            return;
        
        CountDownToCutOff();
    }

    public void SetStartTime()
    {
        if (_isTimerOn)
            return;
        
        _isTimerOn = true;
        _startTime = Time.time;
    }
    
    private void CountDownToCutOff()
    {
        if(!_isTimerOn)
            return;
        
        _elapsedTime = Time.time - _startTime;
        TimeRemaining = cutoffTimeInSeconds - _elapsedTime;

        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0; 
            IsGameOver = true;
        }
        
         
    }
    
    

}

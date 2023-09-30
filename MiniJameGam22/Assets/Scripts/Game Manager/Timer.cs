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
    private float _currentTime;
    private bool _isTimerOn = false;
    private bool _isGameOver = false;

    private void Start()
    {
        //_cutoffTimeInSeconds = 60 * cutoffTimeInMinutes; 
        SetStartTime(); // -> need to call this function when player starts game
    }

    private void Update()
    {
        if(_isGameOver)
            return;
        
        CountDownToCutOff();
    }

    private void SetStartTime()
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

        if (_currentTime - _startTime > cutoffTimeInSeconds)
        {
            _isGameOver = true;
            print("game is over !");
            return;
        }
        
        _currentTime = Time.time;
        float deltaTime = _currentTime - _startTime;
        print("time diff: " + deltaTime);
         
    }
    
    

}

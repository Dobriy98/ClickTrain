using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FuncTimer
{
    private Action action;
    public float timerValue;
    private float firstTimer;
    public FuncTimer(Action action, float timerValue, string key)
    {
        this.action = action;
        this.timerValue = GetTimerFromPrefs(key, timerValue);
        this.firstTimer = timerValue;
    }

    public void Update()
    {
        timerValue -= Time.deltaTime;
        if (timerValue < 0)
        {
            float value = timerValue;
            action();
            timerValue = firstTimer + value;
        }
    }

    public void SetTimerFromPrefs(string key)
    {
        PlayerPrefs.SetFloat(key, timerValue);
    }

    private float GetTimerFromPrefs(string key, float defultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return defultValue;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//this is a script which allows me to call a timer with a certain delay, and invoke another event either at start or at on time
//The bonus is that this script can be called by another timer script to create a looping pattern
public class TimerEvent : MonoBehaviour
{
    public string description;
    public UnityEvent OnTimerReached;
    public UnityEvent OnStartTimer;

    public float waitTime = 1f;
    public bool start;

    public void StartTimer()
    {

        start = true;
        OnStartTimer.Invoke();
    }

    public void OnTime()
    {
        OnTimerReached.Invoke();
    }

    /*void Start()
    {
        StartTimer();
    }*/
    void Update()
    {
        if(start)
        {
            StartCoroutine(TimerRunning(waitTime));
            start = false;
        }
    }

    public IEnumerator TimerRunning(float _duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (elapsedTime >= waitTime)
        {
            OnTime();
            StopCoroutine(TimerRunning(0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//This script is a coroutine which allows me to invoke an event for _repetitions repititions
public class RepeatComponent : MonoBehaviour
{
    float totalSeconds;
    int repetitions;
    public UnityEvent Repeat;
    public UnityEvent RepeatComplete;
    public float delayDebug = 1;
    public float myDelay = 0.5f;

    IEnumerator lastRepeat = null;
    public IEnumerator Repeating(int _repetitions)
    {
        WaitForSeconds wait = new WaitForSeconds(myDelay);
        for (int i = 0; i < _repetitions ; i++)
        {
            Repeat.Invoke();
            totalSeconds += myDelay*100;
            yield return wait;
            if(i== _repetitions) 
            { RepeatComplete.Invoke(); }
        }
        Debug.Log("I repeated "+_repetitions+" times owo");
        Debug.Log("it took me "+ totalSeconds +"time");

    }
    // ...
    public void startRepeat(int _repetitions)
    {

        if (lastRepeat != null)
            StopCoroutine(lastRepeat);
            StartCoroutine(Repeating(_repetitions));
            lastRepeat = Repeating(_repetitions);
    }
    /*
    // Start is called before the first frame update
    void Repeater(int _repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            Repeat.Invoke();
        }
    }
    */
}

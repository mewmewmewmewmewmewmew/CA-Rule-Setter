//This script is useful for the purpose of avoiding visual studio code when it's not necessary
//Invoke any script I want when the game starts

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStart : MonoBehaviour
{
    public UnityEvent GameStarted;
    void Start()
    {
        GameStarted.Invoke();
    }
}

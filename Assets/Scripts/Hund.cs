using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hund : MonoBehaviour
{
    void Start()
    {
        EventManager.current.onPlayerFinishedMove += WatchAlongView;
    }

    private void WatchAlongView()
    {
        Debug.Log("Hund: schaue mich um ...");
    }
}

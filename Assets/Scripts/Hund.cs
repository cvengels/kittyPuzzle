using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hund : MonoBehaviour
{
    void Start()
    {
        EventManager.current.onPlayerFinishMove += WatchAlongView;
    }

    private void WatchAlongView()
    {
        Debug.Log("Hund: schaue mich um ...");
    }
}

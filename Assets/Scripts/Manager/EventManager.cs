﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager current;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    public event Action onSpawnerFinished;
    public void SpawnerFinished()
    {
        if (onSpawnerFinished != null)
        {
            onSpawnerFinished();
        }
    }

    // Nur für Debugging!
    public event Action onMoveTimer;
    public void MoveTimer()
    {
        if (onMoveTimer != null)
        {
            onMoveTimer();
        }
    }


    public event Action onPlayerFinishedMove;
    public void PlayerFinishedMove()
    {
        if (onPlayerFinishedMove != null)
        {
            onPlayerFinishedMove();
        }
    }


    public event Action onPlayerReachedGoal;
    public void PlayerReachedGoal()
    {
        if (onPlayerReachedGoal != null)
        {
            onPlayerReachedGoal();
        }
    }

}

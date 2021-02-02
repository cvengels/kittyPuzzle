using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    private void Start()
    {
        EventManager.current.onPlayerReachedGoal += LevelFinished;
    }

    private void LevelFinished()
    {
        Debug.Log("Level gewonnen!");
        // Load next level
    }
}

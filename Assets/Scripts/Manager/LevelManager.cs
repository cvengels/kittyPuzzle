using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    private float timerForMovement;
    private int timeSinceStart;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    private void Start()
    {
        EventManager.current.onPlayerReachedGoal += LevelFinished;
    }

    private void Update()
    {
        //timerForMovement += Time.deltaTime;
        if (timerForMovement > 2f)
        {
            timeSinceStart += (int)timerForMovement;
            timerForMovement = 0f;
            //Debug.Log("Alle bitte bewegen nach " + timeSinceStart + " Sekunden");
            EventManager.current.MoveTimer();
        }
    }

    private void LevelFinished()
    {
        Debug.Log("Level gewonnen!");
        // Load next level
    }
}

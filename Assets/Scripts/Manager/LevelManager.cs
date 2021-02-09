using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager current;

    public int currentLevel;

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
        EventManager.current.DisablePlayerMovement();
        currentLevel++;

        StartCoroutine(LoadNewLevel());

    }

    IEnumerator LoadNewLevel()
    {
        AudioManager.current.Play("LevelWon");

        yield return new WaitForSeconds(3.0f);


        if (currentLevel != 0)
        {
            try
            {
            SceneManager.LoadScene($"Easy{currentLevel}");
            }
            catch
            {
                Debug.LogError($"Level {currentLevel} konnte nicht geladen werden.");
            }
        }
    }
}

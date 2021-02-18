using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerWatchdog : MonoBehaviour
{
    Animator transition;

    public GameObject[] gameManagers;

    private void Awake()
    {
        FindGameManagersInScene(gameManagers);

        EventManager.current.onPlayerReachedGoal += LevelFinished;
        SceneManager.activeSceneChanged += NewLevelLoaded;
    }

    private void NewLevelLoaded(Scene arg0, Scene arg1)
    {
        transition = GameObject.FindObjectOfType<Animator>().GetComponent<Animator>();
    }

    private void LevelFinished()
    {
        transition.SetTrigger("Start");
    }

    private void Start()
    {
        SetOffsetToCamera();
        transition = GameObject.FindObjectOfType<Animator>().GetComponent<Animator>();
        transition.SetTrigger("End");
    }


    private void FindGameManagersInScene(GameObject[] manager)
    {
        foreach (GameObject go in manager)
        {
            if (!GameObject.Find(go.name) && go != null)
            {
                GameObject tmpManager = (GameObject)Instantiate(go);
                tmpManager.name = go.name;
            }
        }
    }

    private void SetOffsetToCamera()
    {
        // Add a little offset to main camera to try to get rid of 
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 0.001f, Camera.main.transform.position.y + 0.001f, -10f);
    }
}

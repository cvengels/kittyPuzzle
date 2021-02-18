using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerWatchdog : MonoBehaviour
{

    public GameObject[] gameManagers;

    private void Awake()
    {
        FindGameManagersInScene(gameManagers);
    }


    private void Start()
    {
        SetOffsetToCamera();
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

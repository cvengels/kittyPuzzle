using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerWatchdog : MonoBehaviour
{

    public GameObject[] gameManager;

    private void Awake()
    {
        foreach (GameObject go in gameManager)
        {
            if (!GameObject.Find(go.name) && go != null)
            {
                GameObject tmpManager = (GameObject)Instantiate(go);
                tmpManager.name = go.name;
            }
        }
    }

    private void Start()
    {
        SetOffsetToCamera();
    }

    private void SetOffsetToCamera()
    {
        Camera.main.transform.position = new Vector2(0.001f, 0.001f);
    }
}

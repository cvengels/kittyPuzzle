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
}

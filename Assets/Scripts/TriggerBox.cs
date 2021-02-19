using System;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    ObjectGridInteraction gridInteraction;

    string gameObjectOnTop;

    void Awake()
    {
        gridInteraction = GetComponent<ObjectGridInteraction>();
    }

    void Start()
    {
        gameObjectOnTop = "";
    }


    private void PlayerFinishedMove()
    {
        GameObject onTop = gridInteraction.GetPushableObjectOnTop();
        if (onTop != null && gameObjectOnTop != "")
        {
            if (onTop.name != gameObjectOnTop)
            {
                gameObjectOnTop = onTop.name;
            }
        }
    }


    private void SpawnerFinished()
    {
        EventManager.current.AddTriggerBox();
    }


    private void OnEnable()
    {
        EventManager.current.onPlayerFinishedMove += PlayerFinishedMove;
        EventManager.current.onSpawnerFinished += SpawnerFinished;
    }


    private void OnDisable()
    {
        EventManager.current.onPlayerFinishedMove -= PlayerFinishedMove;
        EventManager.current.onSpawnerFinished -= SpawnerFinished;
    }
}

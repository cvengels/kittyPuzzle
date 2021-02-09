using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager current;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovement()
    {
        if (onEnablePlayerMovement != null)
        {
            onEnablePlayerMovement();
        }
    }

    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovement()
    {
        if (onDisablePlayerMovement != null)
        {
            onDisablePlayerMovement();
        }
    }

    public event Action onSpawnerFinished;
    public void SpawnerFinished()
    {
        if (onSpawnerFinished != null)
        {
            onSpawnerFinished();
        }
    }


    public event Action onPlayerStartMove;
    public void PlayerStartMove()
    {
        if (onPlayerStartMove != null)
        {
            onPlayerStartMove();
        }
    }


    public event Action onPlayerFinishMove;
    public void PlayerFinishMove()
    {
        if (onPlayerFinishMove != null)
        {
            onPlayerFinishMove();
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

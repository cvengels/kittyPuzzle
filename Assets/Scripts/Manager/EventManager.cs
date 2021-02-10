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

    public event Action onAddMovingEntity;
    public void AddMovingEntity()
    {
        onAddMovingEntity?.Invoke();
    }

    public event Action onRemoveMovingEntity;
    public void RemoveMovingEntity()
    {
        onRemoveMovingEntity?.Invoke();
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovement()
    {
        onEnablePlayerMovement?.Invoke();
    }

    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovement()
    {
        onDisablePlayerMovement?.Invoke();
    }

    public event Action onSpawnerFinished;
    public void SpawnerFinished()
    {
        onSpawnerFinished?.Invoke();
    }


    public event Action onPlayerStartMove;
    public void PlayerStartMove()
    {
        onPlayerStartMove?.Invoke();
    }


    public event Action onPlayerFinishMove;
    public void PlayerFinishMove()
    {
        onPlayerFinishMove?.Invoke();
    }


    public event Action onPlayerReachedGoal;
    public void PlayerReachedGoal()
    {
        onPlayerReachedGoal?.Invoke();
    }

}

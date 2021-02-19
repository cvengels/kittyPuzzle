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


    public event Action onAddTriggerBox;
    public void AddTriggerBox()
    {
        onAddTriggerBox?.Invoke();
    }


    public event Action onTriggerBoxTouch;
    public void TriggerBoxTouch()
    {
        onTriggerBoxTouch?.Invoke();
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


    public event Action onPlayerFinishedMove;
    public void PlayerFinishedMove()
    {
        onPlayerFinishedMove?.Invoke();
    }


    public event Action onPlayerReachedGoal;
    public void PlayerReachedGoal()
    {
        //Debug.Log("EventManager: Level beendet");
        onPlayerReachedGoal?.Invoke();
    }

}

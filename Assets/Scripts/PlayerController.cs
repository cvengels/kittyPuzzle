using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int movingObjects;
    public int MovingObjects
    {
        get { return movingObjects; }
    }

    [SerializeField]
    private bool playerCanMove;
    public bool PlayerCanMove
    {
        get { return playerCanMove; }
    }

    private Queue<Vector2> savedMoves;
    private bool levelTransition;

    PlayerInput controls;
    ObjectGridInteraction playerReference;


    private void Awake()
    {
        playerReference = GetComponent<ObjectGridInteraction>();

        savedMoves = new Queue<Vector2>();

        controls = new PlayerInput();
        controls.Player.GridMovement.performed += ctx => AddMove(ctx.ReadValue<Vector2>());
        controls.Player.ResetLevel.performed += ctx => ResetLevel();
    }

    private void Start()
    {

    }

    private void LevelFinished()
    {
        levelTransition = true;
        DisablePlayerControl();
        ResetSavedMoves();
    }

    private void ResetLevel()
    {
        ResetSavedMoves();
    }

    private void AddMove(Vector2 newMove)
    {
        savedMoves.Enqueue(newMove);

        if (playerCanMove)
        {
            LookInQueueForNewMove();
        }
    }

    private void LookInQueueForNewMove()
    {
        if (savedMoves.Count > 0)
        {
            if (playerReference.CanMoveOnGrid(savedMoves.Peek()))
            {
                AudioManager.current.Play("CatMoving");
                playerReference.AskToMove(savedMoves.Dequeue());
            }
            else
            {
                savedMoves.Dequeue();
                Debug.Log("Schritt verworfen ...");
                LookInQueueForNewMove();
            }
        }
    }

    public void ResetSavedMoves()
    {
        savedMoves.Clear();
    }

    private void AddMovingEntityInt()
    {
        movingObjects++;
        //Debug.Log("Moving Objects (Add): " + movingObjects);
        if (movingObjects > 0)
        {
            DisablePlayerControl();
        }
    }

    private void RemoveMovingEntityInt()
    {
        movingObjects--;
        //Debug.Log("Moving Objects (Del): " + movingObjects);
        if (movingObjects == 0)
        {
            EnablePlayerControl();
        }
    }

    private void EnablePlayerControl()
    {
        //Debug.Log("Spieler-Steuerung freigegeben!");

        if (!levelTransition)
        {
            playerCanMove = true;
        }
    }

    private void DisablePlayerControl()
    {
        Debug.Log("Spieler-Steuerung gesperrt!");
        playerCanMove = false;
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        controls.Enable();
        EventManager.current.onEnablePlayerMovement += EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement += DisablePlayerControl;
        EventManager.current.onPlayerReachedGoal += LevelFinished;
        EventManager.current.onAddMovingEntity += AddMovingEntityInt;
        EventManager.current.onRemoveMovingEntity += RemoveMovingEntityInt;
        EventManager.current.onPlayerFinishMove += LookInQueueForNewMove;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        levelTransition = false;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        controls.Disable();
        EventManager.current.onEnablePlayerMovement -= EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement -= DisablePlayerControl;
        EventManager.current.onPlayerReachedGoal -= LevelFinished;
        EventManager.current.onAddMovingEntity -= AddMovingEntityInt;
        EventManager.current.onRemoveMovingEntity -= RemoveMovingEntityInt;
        EventManager.current.onPlayerFinishMove -= LookInQueueForNewMove;
    }
}
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
    private bool playerIsStandingStill;
    public bool PlayerCanMove
    {
        get { return playerIsStandingStill; }
    }

    [SerializeField]
    private bool playerMovesHorizontal;
    public bool PlayerMovesHorizontal
    {
        get { return playerMovesHorizontal; }
    }

    [SerializeField]
    private bool playerMovesVertical;
    public bool PlayerMovesVertical
    {
        get { return playerMovesVertical; }
    }

    private bool levelTransition;

    private ObjectGridInteraction playerReference;

    public GameObject portalBox;


    private void Awake()
    {
        playerReference = GetComponent<ObjectGridInteraction>();
        PlayerFinishedMove();
    }


    private void PlayerFinishedMove()
    {
        if (!levelTransition)
        {
            playerMovesHorizontal = false;
            playerMovesVertical = false;
            playerIsStandingStill = true;
        }
    }

    private void Update()
    {
        if (playerIsStandingStill)
        {
            if (Input.GetAxisRaw("Horizontal") != 0f && !playerMovesVertical)
            {
                Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                if (/*playerReference.CanMoveOnGrid(direction) && */playerReference.AskToMove(direction))
                {
                    playerReference.AskToMove(direction);
                    playerMovesHorizontal = true;
                    playerIsStandingStill = false;
                }
            }
            else if (Input.GetAxisRaw("Vertical") != 0f && !playerMovesHorizontal)
            {
                Vector2 direction = new Vector2(0, Input.GetAxisRaw("Vertical"));
                if (/*playerReference.CanMoveOnGrid(direction) && */playerReference.AskToMove(direction))
                {
                    playerReference.AskToMove(direction);
                    playerMovesVertical = true;
                    playerIsStandingStill = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ExchangeAssets();
        }
    }

    private void ExchangeAssets()
    {
        Debug.Log("Magic Magic ...");
        // ToDo
    }

    private void LevelFinished()
    {
        levelTransition = true;
        DisablePlayerControl();
    }


    private void AddMovingEntityInt()
    {
        movingObjects++;
        if (movingObjects > 0)
        {
            DisablePlayerControl();
        }
    }

    private void RemoveMovingEntityInt()
    {
        movingObjects--;
        if (movingObjects == 0)
        {
            EnablePlayerControl();
        }
    }

    private void EnablePlayerControl()
    {
        if (!levelTransition)
        {
            playerIsStandingStill = true;
        }
    }

    private void DisablePlayerControl()
    {
        playerIsStandingStill = false;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        levelTransition = false;
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        EventManager.current.onEnablePlayerMovement += EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement += DisablePlayerControl;
        EventManager.current.onPlayerReachedGoal += LevelFinished;
        EventManager.current.onAddMovingEntity += AddMovingEntityInt;
        EventManager.current.onRemoveMovingEntity += RemoveMovingEntityInt;
        EventManager.current.onPlayerFinishedMove += PlayerFinishedMove;
    }


    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        EventManager.current.onEnablePlayerMovement -= EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement -= DisablePlayerControl;
        EventManager.current.onPlayerReachedGoal -= LevelFinished;
        EventManager.current.onAddMovingEntity -= AddMovingEntityInt;
        EventManager.current.onRemoveMovingEntity -= RemoveMovingEntityInt;
        EventManager.current.onPlayerFinishedMove -= PlayerFinishedMove;
    }
}
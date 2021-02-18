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

    private Animator playerAnimation;



    private void Awake()
    {
        playerReference = GetComponent<ObjectGridInteraction>();
        playerAnimation = GetComponent<Animator>();
        PlayerFinishedMove();
    }


    private void PlayerFinishedMove()
    {
        if (!levelTransition)
        {
            playerAnimation.SetBool("PlayerIsMoving", false);


            playerMovesHorizontal = false;
            playerMovesVertical = false;
            playerCanMove = true;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            if (playerCanMove)
            {
                if (Input.GetAxisRaw("Horizontal") != 0f && !playerMovesVertical)
                {
                    Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                    if (playerReference.AskToMove(direction))
                    {
                        playerReference.AskToMove(direction);
                        playerAnimation.SetFloat("Vertical", 0f);
                        playerAnimation.SetFloat("Horizontal", direction.x);
                        playerAnimation.SetBool("PlayerIsMoving", true);
                        playerMovesHorizontal = true;
                        playerCanMove = false;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") != 0f && !playerMovesHorizontal)
                {
                    Vector2 direction = new Vector2(0, Input.GetAxisRaw("Vertical"));
                    if (playerReference.AskToMove(direction))
                    {
                        playerReference.AskToMove(direction);

                        playerAnimation.SetFloat("Horizontal", 0f);
                        playerAnimation.SetFloat("Vertical", direction.y);

                        playerAnimation.SetBool("PlayerIsMoving", true);
                        playerMovesVertical = true;
                        playerCanMove = false;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelManager.current.ResetLevel();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ExchangeAssets();
            }
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
            playerCanMove = true;
        }
    }

    private void DisablePlayerControl()
    {
        playerCanMove = false;
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
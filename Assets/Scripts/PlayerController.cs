using System;
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

    ObjectGridInteraction playerReference;

    private void Start()
    {
        playerReference = GetComponent<ObjectGridInteraction>();

        EventManager.current.onAddMovingEntity += AddEntityInt;
        EventManager.current.onRemoveMovingEntity += RemoveEntityInt;
    }


    private void AddEntityInt()
    {
        movingObjects++;
        Debug.Log("Moving Objects (Add): " + movingObjects);
        if (movingObjects > 0)
        {
            DisablePlayerControl();
        }
    }
    private void RemoveEntityInt()
    {
        movingObjects--;
        Debug.Log("Moving Objects (Del): " + movingObjects);
        if (movingObjects == 0)
        {
            EnablePlayerControl();
        }
    }

    private void EnablePlayerControl()
    {
        Debug.Log("Spieler-Steuerung freigegeben!");
        playerCanMove = true;
    }
    private void DisablePlayerControl()
    {
        Debug.Log("Spieler-Steuerung gesperrt!");
        playerCanMove = false;
    }


    private void Update()
    {
        if (playerCanMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerReference.AskToMove(Vector2.up, playerReference.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                playerReference.AskToMove(Vector2.left, playerReference.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                playerReference.AskToMove(Vector2.down, playerReference.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerReference.AskToMove(Vector2.right, playerReference.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }


    void OnEnable()
    {
        EventManager.current.onEnablePlayerMovement += EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement += DisablePlayerControl;
    }

    void OnDisable()
    {
        EventManager.current.onEnablePlayerMovement -= EnablePlayerControl;
        EventManager.current.onDisablePlayerMovement -= DisablePlayerControl;
    }

}
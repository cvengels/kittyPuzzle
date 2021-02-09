using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private bool playerCanMove;

    public bool PlayerCanMove
    {
        get { return playerCanMove; }
    }

    ObjectGridInteraction gridInteractor;
    ObjectGridInteraction playerObject;

    private void Start()
    {
        gridInteractor = GetComponent<ObjectGridInteraction>();
        playerObject = GetComponent<ObjectGridInteraction>();
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
                gridInteractor.AskToMove(Vector2.up, playerObject.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                gridInteractor.AskToMove(Vector2.left, playerObject.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                gridInteractor.AskToMove(Vector2.down, playerObject.MyData.moveSpeed);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gridInteractor.AskToMove(Vector2.right, playerObject.MyData.moveSpeed);
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
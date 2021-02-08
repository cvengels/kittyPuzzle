using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ObjectGridInteraction gridInteractor;
    ObjectGridInteraction playerObject;

    private void Start()
    {
        gridInteractor = GetComponent<ObjectGridInteraction>();
        playerObject = GetComponent<ObjectGridInteraction>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gridInteractor.AskToMove(Vector2.up, playerObject.MovementSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            gridInteractor.AskToMove(Vector2.left, playerObject.MovementSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            gridInteractor.AskToMove(Vector2.down, playerObject.MovementSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            gridInteractor.AskToMove(Vector2.right, playerObject.MovementSpeed);
        }
    }

}
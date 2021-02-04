using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ObjectGridInteraction gridInteractor;

    private void Start()
    {
        gridInteractor = GetComponent<ObjectGridInteraction>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gridInteractor.AskToMove(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            gridInteractor.AskToMove(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            gridInteractor.AskToMove(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            gridInteractor.AskToMove(Vector2.right);
        }
    }

}
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(0);
        }
    }

}
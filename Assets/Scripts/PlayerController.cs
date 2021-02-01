using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement controls;
    public float speed = 1;
    Vector3 nextPosition;
    private bool isMoving;

    [SerializeField]
    private Tilemap groundTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;


    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(
                    nextPosition.x,
                    nextPosition.y,
                    0),
                Time.deltaTime * speed);
        }
        if (Vector3.Distance(transform.position, nextPosition) < 0.02 && isMoving)
        {
            isMoving = false;
            transform.position = nextPosition;
        }
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction) && !isMoving)
        {
            nextPosition = transform.position + (Vector3)direction;
            isMoving = true;
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        

        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
        {
            string nameOfTile = collisionTilemap.GetTile(gridPosition).name;
            Debug.Log("Hindernis heißt " + nameOfTile);
            return false;
        }
        return true;
    }

}
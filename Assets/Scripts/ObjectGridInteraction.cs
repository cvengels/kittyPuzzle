using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectGridInteraction : MonoBehaviour
{
    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    private bool isMoving;
    private bool pushing;

    private float newMovementSpeedCalculated;
    private Vector3 newMovePosition;

    private Color gizmoColor;


    [SerializeField]
    private SpawnableObjectBehaviour.PublicProperties myData;
    public SpawnableObjectBehaviour.PublicProperties MyData
    {
        get { return myData; }
        set { myData = value; }
    }


    private int GetSeedFromString(string s)
    {
        string seedInteger = "";

        foreach (char c in s)
        {
            char upper = char.ToUpper(c);
            if (upper < 'A' || upper > 'Z')
            {
                continue;
            }
            else
            {
                int tmp = Convert.ToInt32(upper);
                seedInteger += tmp;
            }
        }
        try
        {
            return Convert.ToInt32(seedInteger);
        }
        catch
        {
            return 0;
        }
    }

    float xVelocity, yVelocity;

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(newMovePosition, 0.25f);
    }

    void Start()
    {
        // Find tilemaps for the player to walk in the level
        if (GameObject.Find("GroundTilemap"))
        {
            groundTilemap = GameObject.Find("GroundTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Boden-Tilemap nicht gefunden!");
        }

        if (GameObject.Find("CollisionTilemap"))
        {
            collisionTilemap = GameObject.Find("CollisionTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Kollisions-Tilemap nicht gefunden!");
        }

        // Set Gizmo color by seeding the object name
        UnityEngine.Random.InitState(GetSeedFromString(myData.nameOfEntity));
        gizmoColor = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1f, 1f);

        // Reset target position
        ResetTargetPosition();

        //EventManager.current.onMoveTimer += MoveInRandomDirection;
    }


    void Update()
    {
        if (isMoving)
        {
            if (pushing) // when pushing one or more objects, this gameobject gets a new speed and moves linearly
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    newMovePosition,
                    newMovementSpeedCalculated * Time.deltaTime);
            }
            else // normal movement, e.g. of player or NPC
            {
                float newPositionX = Mathf.SmoothDamp(transform.position.x, newMovePosition.x, ref xVelocity, .05f);
                float newPositionY = Mathf.SmoothDamp(transform.position.y, newMovePosition.y, ref yVelocity, .05f);
                transform.position = new Vector3(newPositionX, newPositionY, 0);
            }

            // If own position near target position, move directly to new position and quit moving task
            if (Vector3.Distance(transform.position, newMovePosition) < 0.02f)
            {
                isMoving = false;
                pushing = false;
                transform.position = newMovePosition;
                EventManager.current.RemoveMovingEntity();
                if (myData.isPlayable)
                {
                    EventManager.current.PlayerFinishMove();
                }
            }
        }
    }


    public void AskToMove(Vector2 direction)
    {
        if (!isMoving)
        {
            newMovementSpeedCalculated = myData.moveSpeed;
            if (CanMoveOnGrid(direction))
            {
                // set new position to move to and check if other GameObjects are on this position
                newMovePosition = transform.position + (Vector3)direction;
                isMoving = true;
                EventManager.current.AddMovingEntity();
            }
        }
    }


    public bool CanMoveOnGrid(Vector2 direction)
    {
        Vector3Int gridTargetPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridTargetPosition) || collisionTilemap.HasTile(gridTargetPosition))
        {
            return false;
        }
        return true;
    }


    private List<GameObject> CheckForObjectsOnTargetPosition(Vector3 targetPosition)
    {
        // https://answers.unity.com/questions/383671/find-gameobject-at-position.html

        Collider2D[] colliders;
        List<GameObject> gameObjects = new List<GameObject>(); ;

        //Presuming the object you are testing also has a collider 0 otherwise
        colliders = Physics2D.OverlapCircleAll(targetPosition, .2f);
        if (colliders.Length >= 1)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                //This is the game object you collided with
                GameObject go = colliders[i].gameObject;
                gameObjects.Add(go);
            }
        }
        return gameObjects;
    }

    private void ResetTargetPosition()
    {
        newMovePosition = transform.position;
    }
}
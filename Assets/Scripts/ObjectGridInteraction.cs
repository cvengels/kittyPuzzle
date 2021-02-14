using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectGridInteraction : MonoBehaviour
{
    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    private bool isMoving;
    private bool pushing; // if true, use linear movement

    private float newMovementSpeedCalculated;
    private Vector2 oldDirection;
    private Vector3 newMovePosition;
    float xVelocity, yVelocity;

    private Color gizmoColor;


    [SerializeField]
    private SpawnableObjectBehaviour.PublicProperties myData;
    public SpawnableObjectBehaviour.PublicProperties Data
    {
        get { return myData; }
        set { myData = value; }
    }

    // Give every object category a unique gizmo color
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


    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(newMovePosition, 0.25f);
    }

    void Start()
    {
        // Find ground tilemaps
        if (GameObject.Find("GroundTilemap"))
        {
            groundTilemap = GameObject.Find("GroundTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Boden-Tilemap nicht gefunden!");
        }

        // Find wall tilemaps
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
                float newPositionX = Mathf.SmoothDamp(transform.position.x, newMovePosition.x, ref xVelocity, 1 / myData.moveSpeed);
                float newPositionY = Mathf.SmoothDamp(transform.position.y, newMovePosition.y, ref yVelocity, 1 / myData.moveSpeed);
                transform.position = new Vector3(newPositionX, newPositionY, 0);
            }

            // If own position near target position, move directly to new position and quit moving task
            if (Vector3.Distance(transform.position, newMovePosition) < 0.01f)
            {
                isMoving = false;
                pushing = false;
                transform.position = newMovePosition;
                oldDirection = Vector2.zero;
                EventManager.current.RemoveMovingEntity();
                if (myData.isPlayable)
                {
                    EventManager.current.PlayerFinishedMove();
                }
            }
        }
    }


    public bool AskToMove(Vector2 direction)
    {
        if (!isMoving)
        {
            if (CanMoveOnGrid(direction))
            {
                newMovementSpeedCalculated = myData.moveSpeed;
                newMovePosition = transform.position + (Vector3)direction;
                GameObject[] objectsOnTileInDirection = CheckForObjectsOnTargetPosition(newMovePosition);
                // Set new position to move to and check if other GameObjects are on this position
                if (objectsOnTileInDirection.Length == 0)
                {
                    isMoving = true;

                    if (myData.isPlayable)
                    {
                        AudioManager.current.Play("CatMoving");
                    }
                    else if (myData.isPushable)
                    {
                        AudioManager.current.Play("BoxPush");
                    }

                    EventManager.current.AddMovingEntity();
                    if (myData.isPlayable)
                    {
                        EventManager.current.PlayerStartMove();
                    }
                    return true;
                }
                else // One or more objects found in front of me
                {
                    if (direction != oldDirection && myData.isPlayable)
                    {
                        List<GameObject[]> objectsInPathQueue = ObjectsInMovePath(newMovePosition, direction);
                        string objectString = "{ ";

                        // Each GameObject Array on individual Tiles
                        for (int i = 0; i < objectsInPathQueue.Count; i++)
                        {
                            objectString += "[";
                            // Each GameObject in Array on Tile
                            for (int j = 0; j < objectsInPathQueue[i].Length; j++)
                            {
                                objectString += objectsInPathQueue[i][j].name;
                                if (j < objectsInPathQueue[j].Length - 1)
                                {
                                    objectString += ", ";
                                }
                                else
                                {
                                    objectString += "]";
                                }
                            }
                            if (i < objectsInPathQueue.Count - 1)
                            {
                                objectString += ", ";
                            }
                            else
                            {
                                objectString += " }";
                            }
                        }

                        Debug.LogWarning($"({myData.nameOfEntity}) Objekte vor mir: {objectString} in Richtung ({direction.x}, {direction.y})");
                        oldDirection = direction;
                    }
                }
            }
        }
        return false;
    }


    public bool CanMoveOnGrid(Vector2 direction, Vector3 customPosition = default)
    {
        customPosition = customPosition != default ? customPosition : transform.position;
        Vector3Int gridTargetPosition = groundTilemap.WorldToCell(customPosition + (Vector3)direction);
        if (!groundTilemap.HasTile(gridTargetPosition) || collisionTilemap.HasTile(gridTargetPosition))
        {
            return false;
        }
        return true;
    }

    private List<GameObject[]> ObjectsInMovePath(Vector3 position, Vector2 direction)
    {
        Vector3 nextPosition = position;
        List<GameObject[]> objectQueue = new List<GameObject[]>();
        while (CheckForObjectsOnTargetPosition(position).Length > 0)
        {
            GameObject[] nextObjects = CheckForObjectsOnTargetPosition(nextPosition);
            if (nextObjects.Length > 0 && CanMoveOnGrid(direction))
            {
                objectQueue.Add(nextObjects);
                nextPosition += (Vector3)direction;
            }
        }
        objectQueue.Add(null);

        return objectQueue;
    }


    private GameObject[] CheckForObjectsOnTargetPosition(Vector3 targetPosition)
    {
        // https://answers.unity.com/questions/383671/find-gameobject-at-position.html

        Collider2D[] colliders;
        GameObject[] gameObjects;

        //Presuming the object you are testing also has a collider 0 otherwise
        colliders = Physics2D.OverlapCircleAll(targetPosition, .2f);
        gameObjects = new GameObject[colliders.Length];
        if (colliders.Length >= 1)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                //This is the game object you collided with
                GameObject go = colliders[i].gameObject;
                if (!go.CompareTag("Player"))
                {
                    gameObjects[i] = go;
                }
            }
        }
        return gameObjects;
    }


    private void ResetTargetPosition()
    {
        newMovePosition = transform.position;
    }


}
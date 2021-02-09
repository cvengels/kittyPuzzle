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


    void FixedUpdate()
    {
        if (isMoving)
        {
            if (pushing) // when pushing one or more objects, this gameobject gets a new speed and moves linearly
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    newMovePosition,
                    newMovementSpeedCalculated * Time.fixedDeltaTime);
            }
            else // normal movement, e.g. of player or NPC
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    Vector3.Lerp(transform.position, newMovePosition, myData.moveSpeed / 10),
                    newMovementSpeedCalculated * Time.fixedDeltaTime);
            }

            // If own position near target position, move directly to new position and quit moving task
            if (Vector3.Distance(transform.position, newMovePosition) < 0.02f)
            {
                isMoving = false;
                pushing = false;
                transform.position = newMovePosition;

                if (transform.CompareTag("Player"))
                {
                    EventManager.current.PlayerFinishMove();
                }
            }
        }
    }


    public float AskToMove(Vector2 direction, float forwardedSpeed, int pushOrder = 0)
    {
        if (myData.isPlayable || (myData.isPushable && !myData.isTrigger))
        {
            if (!isMoving)
            {
                newMovementSpeedCalculated = myData.moveSpeed;
                if (CanMoveOnGrid(direction))
                {
                    // set new position to move to and check if other GameObjects are on this position
                    newMovePosition = transform.position + (Vector3)direction;
                    List<GameObject> objectsInPath = CheckForObjectsOnTargetPosition(newMovePosition);

                    if (objectsInPath.Count == 0)
                    {
                        isMoving = true;
                        pushing = false;
                        AudioManager.current.Play("CatMoving");
                        return newMovementSpeedCalculated;
                    }
                    else if (objectsInPath.Count == 1)
                    {
                        // Get reference of object in front of this
                        ObjectGridInteraction objectInFrontOfMe = objectsInPath[0].GetComponent<ObjectGridInteraction>();
                        // If GameObject is player, you are first in push order. Otherwise increment value
                        pushOrder = this.gameObject.CompareTag("Player") ? 0 : pushOrder++;

                        // Is object in front of me pushable?
                        if (objectInFrontOfMe.MyData.isPushable)
                        {
                            // get movement speed of object in front of me
                            newMovementSpeedCalculated = objectInFrontOfMe.AskToMove(direction, newMovementSpeedCalculated, pushOrder);
                            newMovementSpeedCalculated = Mathf.Min(newMovementSpeedCalculated, myData.moveSpeed);

                            if (newMovementSpeedCalculated > 0f)
                            {
                                isMoving = true;
                                pushing = true;
                                AudioManager.current.Play("BoxPush");
                                return newMovementSpeedCalculated;
                            }
                            else
                            {
                                ResetTargetPosition();
                                return 0f;
                            }
                        }
                    }
                    else // more than one object, e.g. a button, hole and a box on top
                    {
                        //ToDo
                    }
                }
                else
                {
                    ResetTargetPosition();
                    return 0f;
                }
            }
            return newMovementSpeedCalculated;
        }
        return 0f;
    }


    private void ResetTargetPosition()
    {
        newMovePosition = transform.position;
    }


    private bool CanMoveOnGrid(Vector2 direction)
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


    /*
    private void MoveInRandomDirection()
    {
        //Debug.Log(this.name + " versucht sich zu bewegen...");
        int randomAxis = Random.Range(0, 2);
        int randomMovement = Random.Range(-1, 2);
        if (randomAxis == 0)
        {
            //Debug.Log(this.name + ": Bewege mich entweder links oder rechts um " + randomMovement);
            AskToMove(new Vector2(randomMovement, 0f));
        }
        else if (randomAxis == 1)
        {
            //Debug.Log(this.name + ": Bewege mich entweder hoch oder runter um " + randomMovement);
            AskToMove(new Vector2(0f, randomMovement));
        }
    }
    */
}
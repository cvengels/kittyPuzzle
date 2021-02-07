using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectGridInteraction : MonoBehaviour
{
    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

    private bool isMoving;
    private bool pushing;
    private float movementSpeedOverwrite;

    private Vector3 newMovePosition;

    // Properties of spawned entity
    [SerializeField]
    private string nameOfObject;
    public string NameOfObject
    {
        get { return nameOfObject; }
        set { nameOfObject = value; }
    }
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    [SerializeField]
    private bool isPlayable;
    public bool IsPlayable
    {
        get { return isPlayable; }
        set { isPlayable = value; }
    }
    [SerializeField]
    private bool isVulnerable;
    public bool IsVulnerable
    {
        get { return isVulnerable; }
        set { isVulnerable = value; }
    }
    [SerializeField]
    private bool isMassive;
    public bool IsMassive
    {
        get { return isMassive; }
        set { isMassive = value; }
    }
    [SerializeField]
    private bool isPushable;
    public bool IsPushable
    {
        get { return isPushable; }
        set { isPushable = value; }
    }
    [SerializeField]
    private bool isHeavy;
    public bool IsHeavy
    {
        get { return isHeavy; }
        set { isHeavy = value; }
    }
    [SerializeField]
    private bool isHole;
    public bool IsHole
    {
        get { return isHole; }
        set { isHole = value; }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(newMovePosition, 0.25f);
    }

    void Start()
    {
        // Find tilemaps for the player to walk in the level
        if (GameObject.Find("Grid/GroundTilemap"))
        {
            groundTilemap = GameObject.Find("Grid/GroundTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Boden-Tilemap nicht gefunden!");
        }
        if (GameObject.Find("Grid/CollisionTilemap"))
        {
            collisionTilemap = GameObject.Find("Grid/CollisionTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Kollisions-Tilemap nicht gefunden!");
        }

        //EventManager.current.onMoveTimer += MoveInRandomDirection;
    }


    void Update()
    {
        if (isMoving)
        {
            if (!pushing) // normal movement of e.g. player
            {
                transform.position = Vector3.Lerp(transform.position, newMovePosition, movementSpeed * Time.deltaTime);
            }
            else // when pushing one or more objects, this gameobject gets a new speed and moves linearly
            {
                transform.position = Vector3.MoveTowards(transform.position, newMovePosition, movementSpeedOverwrite * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, newMovePosition) < 0.02f)
            {
                isMoving = false;
                pushing = false;
                transform.position = newMovePosition;

                if (this.transform.CompareTag("Player"))
                {
                    EventManager.current.PlayerFinishMove();
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
                newMovePosition = transform.position + (Vector3)direction;
                if (CheckForObjectsOnTarget(newMovePosition).Count == 0)
                {
                    //Debug.Log(this.name + " soll von " + transform.position.ToString() + " zu " + newMovePosition.ToString());
                    isMoving = true;
                    if (this.transform.CompareTag("Player"))
                    {
                        EventManager.current.PlayerStartMove();
                    }
                    return true;

                }
                else
                {
                    List<GameObject> objectsInPath = CheckForObjectsOnTarget(newMovePosition);
                    /*
                    string objectNames = "";
                    foreach (GameObject go in objectsInPath)
                    {
                        objectNames += go.name + " ";
                    }
                    Debug.Log(this.name + ": Objekte vor mir: " + objectNames);
                    */
                    if (objectsInPath.Count == 1)
                    {
                        ObjectGridInteraction objectInFrontOfMe = objectsInPath[0].GetComponent<ObjectGridInteraction>();
                        if (objectInFrontOfMe.AskToMove(direction) && objectInFrontOfMe.isPushable)
                        {
                            movementSpeedOverwrite = objectInFrontOfMe.movementSpeed < this.movementSpeed ? 
                                objectInFrontOfMe.movementSpeed : this.movementSpeed;
                            isMoving = true;
                            pushing = true;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    private bool CanMoveOnGrid(Vector2 direction)
    {
        Vector3Int gridTargetPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridTargetPosition) || collisionTilemap.HasTile(gridTargetPosition))
        {
            //string nameOfTile = collisionTilemap.GetTile(gridPosition).name;
            //Debug.Log("Hindernis heißt " + nameOfTile);
            return false;
        }
        return true;
    }


    private List<GameObject> CheckForObjectsOnTarget(Vector3 targetPosition)
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
                if (!go.GetComponent<PositionTarget>())
                {
                    gameObjects.Add(go);
                }
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
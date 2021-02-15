using System;
using System.Linq;
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
    private bool isSpeedOverwritten;

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

        if (myData.isPushable || myData.isHeavy)
        {
            pushing = true;
        }
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
                if (myData.isPushable || myData.isHeavy)
                {
                    pushing = true;
                }
                else
                {
                    pushing = false;
                }
                transform.position = newMovePosition;
                oldDirection = Vector2.zero;
                isSpeedOverwritten = false;
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
                if (!isSpeedOverwritten)
                {
                    newMovementSpeedCalculated = myData.moveSpeed;
                }
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
                        if (!myData.isHeavy)
                        {
                            AudioManager.current.Play("BoxPush", 0.8f);
                        }
                        else
                        {
                            AudioManager.current.Play("BoxPush", 0.5f);
                        }
                    }

                    EventManager.current.AddMovingEntity();
                    if (myData.isPlayable)
                    {
                        EventManager.current.PlayerStartMove();
                    }
                    return true;
                }

                // One or more objects found in front of me
                else
                {
                    if (direction != oldDirection && myData.isPlayable /* || myData.isNPC */)
                    {
                        // Get list of gameobjects in line of sight
                        List<GameObject[]> listOfObjectsInPath = ObjectsInMovePath(transform.position, direction);
                        // Filter for movable objects, and check for heavy things
                        List<GameObject> movableObjects = GetMovableObjectsFromList(listOfObjectsInPath);
                        if (movableObjects.Count > 0)
                        {
                            foreach (GameObject go in movableObjects)
                            {
                                ObjectGridInteraction tmp = go.GetComponent<ObjectGridInteraction>();
                                newMovementSpeedCalculated = Mathf.Min(myData.moveSpeed, tmp.Data.moveSpeed);
                                tmp.OverwriteMovementSpeed(newMovementSpeedCalculated);
                            }
                            foreach (GameObject go in movableObjects)
                            {
                                go.GetComponent<ObjectGridInteraction>().AskToMove(direction);
                            }
                            isMoving = true;
                            pushing = true;
                        }

                        // Switch to walk on / push objects on it
                        else if (listOfObjectsInPath[0].Length == 1 && listOfObjectsInPath[0][0].GetComponent<ObjectGridInteraction>().Data.isTrigger)
                        {
                            isMoving = true;
                        }
                        else
                        {
                            oldDirection = direction;
                        }
                    }
                    else if (myData.isPushable || myData.isHeavy)
                    {
                        isMoving = true;
                        pushing = true;
                    }
                }
            }
        }
        return false;
    }

    public void OverwriteMovementSpeed(float newMaximumMoveSpeed)
    {
        newMovementSpeedCalculated = newMaximumMoveSpeed;
        isSpeedOverwritten = true;
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
        Vector2 nextPosition = position;
        int safetyLine = 0;
        List<GameObject[]> objectQueue = new List<GameObject[]>();
        string tmpStr = myData.nameOfEntity;

        do
        {
            GameObject[] tmpObj = CheckForObjectsOnTargetPosition(nextPosition + direction);
            tmpStr += " -> ";

            if (CanMoveOnGrid(direction, nextPosition))
            {

                if (tmpObj.Length > 0)
                {
                    tmpStr += "[ ";
                    foreach (GameObject go in tmpObj)
                    {
                        tmpStr += go.name + " ";
                    }
                    tmpStr += "]";
                }
                else
                {
                    tmpStr += " [ empty ] ";
                }
            }
            else
            {
                tmpStr += " #Wall#";
            }

            nextPosition += direction;
            safetyLine++;

            objectQueue.Add(tmpObj);

        } while (CanMoveOnGrid(direction, nextPosition) && safetyLine < 100);

        //Debug.LogWarning(tmpStr);

        return objectQueue;
    }


    private List<GameObject> GetMovableObjectsFromList(List<GameObject[]> list)
    {
        List<GameObject> newList = new List<GameObject>();
        List<GameObject> pushList = new List<GameObject>();
        bool listValidated = true;

        // Filter original list (only pushable objects)
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Length > 0)
            {
                foreach (GameObject go in list[i])
                {
                    if (go.GetComponent<ObjectGridInteraction>().Data.isPushable)
                    {
                        newList.Add(go);
                    }
                    else
                    {
                        newList.Add(null);
                    }
                }
            }
            else
            {
                newList.Add(null);
            }
        }

        if (newList.Count > 0)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                if (newList[i] != null && i < newList.Count - 1)
                {
                    if (newList[i + 1] != null)
                    {
                        listValidated = false;
                        pushList.Add(newList[i]);
                        continue;
                    }
                    else
                    {
                        listValidated = true;
                        pushList.Add(newList[i]);
                        break;
                    }
                }
                else
                {
                    listValidated = false;
                    break;
                }
            }
        }

        // Look for ONE heavy object
        if (pushList.Count == 1 && listValidated && pushList[0].GetComponent<ObjectGridInteraction>().Data.isHeavy)
        {
            listValidated = true;
        }
        else if (pushList.Count > 1 && listValidated)
        {
            for (int i = 0; i < pushList.Count; i++)
            {
                if (pushList[i] != null && pushList[i].GetComponent<ObjectGridInteraction>().Data.isHeavy)
                {
                    listValidated = false;
                    break;
                }
            }
        }

        if (!listValidated)
        {
            Debug.LogWarning("Liste nicht ok!");
            pushList.Clear();
        }
        else
        {
            //Debug.LogWarning("Liste hat " + pushList.Count + " Einträge");
            if (pushList.Count > 0)
            {
                string tmpStr = "bewegliche Objekte: " + myData.nameOfEntity;

                for (int i = 0; i < pushList.Count; i++)
                {
                    if (pushList[i] != null)
                    {
                        tmpStr += " -> [ " + pushList[i].name + " ]";
                    }
                    else
                    {
                        tmpStr += " -> [ empty ]";
                    }
                }
                Debug.LogWarning(tmpStr);
            }
        }

        return pushList;
    }


    private GameObject[] CheckForObjectsOnTargetPosition(Vector3 targetPosition)
    {
        // https://answers.unity.com/questions/383671/find-gameobject-at-position.html

        Collider2D[] colliders;
        GameObject[] gameObjects;

        //Presuming the object you are testing also has a collider 0 otherwise
        colliders = Physics2D.OverlapCircleAll(targetPosition, .2f);
        gameObjects = new GameObject[colliders.Length];
        if (colliders.Length > 0)
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
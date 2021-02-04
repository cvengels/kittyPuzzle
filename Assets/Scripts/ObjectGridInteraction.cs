using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectGridInteraction : MonoBehaviour
{
    public PositionTarget positionTarget;
    bool isMoving;

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

    void Start()
    {
        positionTarget = Instantiate(positionTarget);
        positionTarget.name = this.name + "PositionTarget";
        positionTarget.transform.parent = GameObject.Find(this.NameOfObject + "Container").transform;
        positionTarget.transform.position = this.transform.position;

        //EventManager.current.onMoveTimer += MoveInRandomDirection;
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

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, positionTarget.transform.position, MovementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, positionTarget.transform.position) < 0.02f)
            {
                isMoving = false;
                transform.position = positionTarget.transform.position;

                EventManager.current.PlayerFinishedMove();
            }
        }
    }

    public bool AskToMove(Vector2 direction)
    {
        if (!isMoving)
        {
            if (positionTarget.CanMoveOnGrid(direction))
            {
                positionTarget.MoveInDirection(direction);
                if (positionTarget.CheckForObjectsInPath().Count == 0)
                {
                    //Debug.Log(this.name + " soll von " + transform.position.ToString() + " zu " + positionTarget.transform.position.ToString());
                    isMoving = true;
                    return true;

                }
                else
                {
                    List<GameObject> objectsInPath = positionTarget.CheckForObjectsInPath();
                    string objectNames = "";
                    foreach (GameObject go in objectsInPath)
                    {
                        objectNames += go.name + " ";
                    }
                    Debug.Log(this.name + ": Objekte vor mir: " + objectNames);
                    positionTarget.MoveBack();
                    return false;
                }
            }
        }
        return false;
    }
}
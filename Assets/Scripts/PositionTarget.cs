using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PositionTarget : MonoBehaviour
{
    [SerializeField]
    private float lastPositionX;
    public float LastPositionX
    {
        get { return lastPositionX; }
    }
    [SerializeField]
    private float lastPositionY;
    public float LastPositionY
    {
        get { return lastPositionY; }
    }

    private Tilemap groundTilemap;
    private Tilemap collisionTilemap;

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
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }

    public void MoveInDirection(Vector2 direction)
    {
        lastPositionX = transform.position.x;
        lastPositionY = transform.position.y;
        transform.position += (Vector3)direction;
    }

    public void MoveBack()
    {
        transform.position = new Vector3(lastPositionX, lastPositionY, 0f);
    }

    public List<GameObject> CheckForObjectsInPath()
    {
        // https://answers.unity.com/questions/383671/find-gameobject-at-position.html

        Collider2D[] colliders;
        List<GameObject> gameObjects = new List<GameObject>(); ;

        //Presuming the object you are testing also has a collider 0 otherwise
        colliders = Physics2D.OverlapCircleAll(transform.position, .2f);
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

    public bool CanMoveOnGrid(Vector2 direction)
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
}

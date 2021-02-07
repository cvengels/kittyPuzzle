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



    void Start()
    {
        
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

 

    
}

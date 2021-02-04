using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "SpawnableObject", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnableObjectBehaviour : ScriptableObject
{
    // GameObject and Tile 
    public string nameOfEntity;
    public GameObject spawnableGameObject;
    public Tile spawnerTile;
    public int minimumSpawners;
    public int maximumSpawners;
    public float moveSpeed = 30f;

    // Properties of spawned entity
    public bool isPlayable;
    public bool isVulnerable;
    public bool isMassive;
    public bool isPushable;
    public bool isHeavy;
    public bool isHole;
   

}

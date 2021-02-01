using System.Collections;
using System.Collections.Generic;
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

    // Properties of spawned entity
    public bool isPlayable;
    public bool isPushable;
    public bool isDangerous;

}

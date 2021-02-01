using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelApprover : MonoBehaviour
{
    public Tilemap spawnMap;

    public SpawnableObjectBehaviour[] spawnableGameBojects;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SearchForSpawners(spawnMap, spawnableGameBojects);
    }

    void SearchForSpawners(Tilemap tileMap, SpawnableObjectBehaviour[] spawnableObjects)
    {
        if (spawnableGameBojects.Length > 0)
        {

            foreach (SpawnableObjectBehaviour spawner in spawnableObjects)
            {
                Debug.Log("Objekte vom Typ " + spawner.nameOfEntity + " werden gesucht ...");

                if (spawnMap.ContainsTile(spawner.spawnerTile))
                {
                    Debug.Log("Objekte vom Typ " + spawner.nameOfEntity + " wurden gefunden!");
                    BoundsInt bounds = tileMap.cellBounds;
                    TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

                    for (int x = 0; x < bounds.size.x; x++)
                    {
                        for (int y = 0; y < bounds.size.y; y++)
                        {
                            TileBase tile = allTiles[x + y * bounds.size.x];
                            if (tile != null)
                            {
                                if (tile == spawner.spawnerTile)
                                {
                                    Debug.Log("x:" + x + " y:" + y + " Objekt: " + spawner.nameOfEntity);
                                    Instantiate(spawner.spawnableGameObject, new Vector3(tileMap.origin.x + x + 0.5f, tileMap.origin.y + y + 0.5f, 0f), Quaternion.identity);
                                }
                            }
                        }
                    }
                }
            }
            spawnMap.ClearAllTiles();
        }
    }

    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
    }
}

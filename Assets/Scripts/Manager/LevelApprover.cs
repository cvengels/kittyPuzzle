﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelApprover : MonoBehaviour
{

    public static LevelApprover current;

    private Tilemap spawnMap;
    public SpawnableObjectBehaviour[] spawnableGameObjects;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }

    void Start()
    {
        // Find tilemaps for the player to walk in the level
        if (GameObject.Find("Grid/SpawnerTilemap"))
        {
            spawnMap = GameObject.Find("Grid/SpawnerTilemap").GetComponent<Tilemap>();
        }
        else
        {
            Debug.LogError("Spawner-Tilemap nicht gefunden!");
        }

        SearchForSpawners(spawnMap, spawnableGameObjects);
    }

    void SearchForSpawners(Tilemap tileMap, SpawnableObjectBehaviour[] spawnableObjects)
    {
        if (spawnableGameObjects.Length > 0)
        {
            foreach (SpawnableObjectBehaviour spawner in spawnableObjects)
            {
                if (spawner != null)
                {
                    if (spawner.name == "" ||
                        spawner.spawnableGameObject == null ||
                        spawner.spawnerTile == null ||
                        (spawner.minimumSpawners > spawner.maximumSpawners ||
                            (spawner.minimumSpawners > 0 && spawner.maximumSpawners == 0)))
                    {
                        Debug.LogWarning("Spawner \"" + spawner.nameOfEntity + "\" übersprungen, fehlerhafte Einträge im Inspector!");
                        continue;
                    }
                    Debug.Log("Objekte vom Typ " + spawner.nameOfEntity + " werden gesucht ...");

                    if (tileMap.ContainsTile(spawner.spawnerTile))
                    {
                        GameObject tmpParent = new GameObject();
                        tmpParent.name = spawner.name + "Container";

                        int countTiles = 0;
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
                                        countTiles++;
                                        //Debug.Log("x:" + x + " y:" + y + " Objekt: " + spawner.nameOfEntity);
                                        GameObject tmpChild = Instantiate(spawner.spawnableGameObject, new Vector3(tileMap.origin.x + x + 0.5f, tileMap.origin.y + y + 0.5f, 0f), Quaternion.identity);
                                        tmpChild.name = spawner.name + countTiles;

                                        //Instanciate one parent GameObject to sort all spawned children
                                        tmpChild.transform.parent = tmpParent.transform;
                                    }
                                }
                            }
                        }
                        Debug.Log(countTiles + "x " + spawner.nameOfEntity + " gespawnt");
                    }
                }
            }
            Debug.Log("Spawner-Suche beendet");
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

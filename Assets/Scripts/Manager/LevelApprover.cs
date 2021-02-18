using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelApprover : MonoBehaviour
{

    public static LevelApprover current;

    private Tilemap spawnMap;
    public SpawnableObjectBehaviour[] spawnableGameObjects;

    private bool spawnerSearchFinished;


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        current = this;
    }


    void SearchForSpawners(Tilemap tileMap, SpawnableObjectBehaviour[] spawnableObjects)
    {
        // Are spawnable scripted objects added to the array in the inspector?
        if (spawnableGameObjects.Length > 0)
        {
            foreach (SpawnableObjectBehaviour spawner in spawnableObjects)
            {
                // Skip empty entries
                if (spawner != null)
                {
                    // validate basic spanwer values
                    if (spawner.NameOfEntity == "" ||
                        spawner.spawnableGameObject == null ||
                        spawner.spawnerTile == null ||
                        spawner.MoveSpeed < 0f)
                    {
                        Debug.LogWarning("Spawner \"" + spawner.NameOfEntity + "\" übersprungen, fehlerhafte Einträge im Inspector!");
                        continue;
                    }

                    // Everything ok with the spawner values
                    int countTiles = 0;
                    //Debug.Log("Objekte vom Typ " + spawner.NameOfEntity + " werden gesucht ...");

                    if (tileMap != null && spawner != null)
                    {
                        // check if tilemap has tiles of the spawner type
                        if (tileMap.ContainsTile(spawner.spawnerTile))
                        {
                            //Instanciate one parent GameObject to sort all spawned children
                            GameObject tmpParent = new GameObject
                            {
                                name = spawner.name + "Container"
                            };

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
                                            GameObject tmpChild = Instantiate(spawner.spawnableGameObject, new Vector3(tileMap.origin.x + x + tileMap.tileAnchor.x, tileMap.origin.y + y + tileMap.tileAnchor.y, 0f), Quaternion.identity);
                                            tmpChild.name = spawner.name + countTiles;

                                            // Set spawner properties to instance properties
                                            if (tmpChild.GetComponent<ObjectGridInteraction>())
                                            {
                                                ObjectGridInteraction tmpGridInteraction = tmpChild.GetComponent<ObjectGridInteraction>();
                                                tmpGridInteraction.Data = new SpawnableObjectBehaviour.PublicProperties(
                                                    spawner.NameOfEntity.Replace(" ", ""),
                                                    spawner.MoveSpeed,
                                                    spawner.IsPlayable,
                                                    spawner.IsVulnerable,
                                                    spawner.IsPushable,
                                                    spawner.IsHeavy,
                                                    spawner.IsHole,
                                                    spawner.IsTrigger);
                                            }

                                            // Add child object to parent
                                            tmpChild.transform.parent = tmpParent.transform;
                                        }
                                    }
                                }
                            }
                            // normal (ignore maximum spawn value, you can add as many spawners as you like)
                            if (spawner.minimumSpawners >= 0 && spawner.maximumSpawners == 0)
                            {
                                if (countTiles >= spawner.minimumSpawners)
                                {
                                    //Debug.Log(countTiles + "x " + spawner.NameOfEntity + " gespawnt");
                                }
                            }
                            // spawn count has to be specific (e.g. start tile)
                            else if (spawner.minimumSpawners > 0 && spawner.maximumSpawners > 0)
                            {
                                // Nothing is ok
                                if (countTiles < spawner.minimumSpawners || countTiles > spawner.maximumSpawners)
                                {
                                    Debug.LogError(countTiles + "x " + spawner.NameOfEntity + " gespawnt, erlaubt sind Min "
                                        + spawner.minimumSpawners + "x und Max " + spawner.maximumSpawners);

                                }
                                // Everything is ok
                                else if (countTiles >= spawner.minimumSpawners && countTiles <= spawner.maximumSpawners)
                                {
                                    //Debug.Log(countTiles + "x " + spawner.NameOfEntity + " gespawnt");
                                }
                            }
                        }
                    }
                    else if (spawner == null)
                    {
                        Debug.LogError("Spawner existiert nicht!");
                    }
                    else if (tileMap == null)
                    {
                        Debug.LogError("Tilemap existiert nicht!");
                    }
                }
            }
            Debug.Log("Spawner-Suche beendet");
            spawnMap.ClearAllTiles();
            EventManager.current.SpawnerFinished();
            EventManager.current.EnablePlayerMovement();
        }
        else
        {
            Debug.LogError("Keine Einträge für Spawner im Level Approver gefunden. Bitte ergänzen!");
        }
    }

    void OnEnable()
    {
        // Whenever a scene is loaded call OnSceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Whenever a scene is loaded call OnSceneLoaded
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene != SceneManager.GetSceneByName("MainMenu"))
        {
            //Debug.Log("OnSceneLoaded: " + scene.name);

            // Find tilemaps for the player and interactive elements on the grid
            if (GameObject.Find("SpawnerTilemap"))
            {
                spawnMap = GameObject.Find("SpawnerTilemap").GetComponent<Tilemap>();
            }
            else
            {
                Debug.LogError("Spawner-Tilemap nicht gefunden!");
            }

            if (spawnMap != null && spawnableGameObjects != null)
            {
                SearchForSpawners(spawnMap, spawnableGameObjects);
            }
            else if (spawnMap == null)
            {
                Debug.LogError("Tilemap existiert nicht!");
            }
            else if (spawnableGameObjects == null)
            {
                Debug.LogError("Spawner existieren nicht!");
            }
        }
    }
}

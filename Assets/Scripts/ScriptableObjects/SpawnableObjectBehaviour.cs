using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "SpawnableGridObject", menuName = "ScriptableObjects/SpawnManagerScriptableObjects", order = 1)]
public class SpawnableObjectBehaviour : ScriptableObject
{

    [System.Serializable]
    public struct PublicProperties
    {
        // Properties of spawned entity
        public string nameOfEntity;
        public float moveSpeed;
        public bool isPlayable;
        public bool isVulnerable;
        public bool isPushable;
        public bool isHeavy;
        public bool isHole;
        public bool isTrigger;

        public PublicProperties(string nameOfEntity, float moveSpeed, 
            bool isPlayable, bool isVulnerable, bool isPushable, bool isHeavy, bool isHole, bool isTrigger)
        {
            this.nameOfEntity = nameOfEntity;
            this.moveSpeed = moveSpeed;
            this.isPlayable = isPlayable;
            this.isVulnerable = isVulnerable;
            this.isPushable = isPushable;
            this.isHeavy = isHeavy;
            this.isHole = isHole;
            this.isTrigger = isTrigger;
        }
    }

    public PublicProperties publicProperties;
    public string NameOfEntity => publicProperties.nameOfEntity;
    public float MoveSpeed => publicProperties.moveSpeed;
    public bool IsPlayable => publicProperties.isPlayable;
    public bool IsVulnerable => publicProperties.isVulnerable;
    public bool IsPushable => publicProperties.isPushable;
    public bool IsHeavy => publicProperties.isHeavy;
    public bool IsHole => publicProperties.isHole;
    public bool IsTrigger => publicProperties.isTrigger;

    // GameObject and Tile (for LevelApprover)
    public GameObject spawnableGameObject;
    public Tile spawnerTile;
    public int minimumSpawners;
    public int maximumSpawners;
    



}

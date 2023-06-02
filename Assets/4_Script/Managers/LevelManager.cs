using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager
{
    public static LevelManager Instance;

    [Header("Levels")]
    [SerializeField] private List<Level> levels;

    [Header("SpawnPosition")]
    public List<Transform> positionToEnemySpawn;
    public Transform playerPositionToSpawn;
    public List<Transform> positionsToVillegerWalkAndSpawn;

    public override void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public Level GetCurrentLevel()
    {
        return levels[0];
    }

    public Transform GetRandomPositionForEnemySpawn()
    {
        return positionToEnemySpawn[Random.Range(0, positionToEnemySpawn.Count)];
    }
    public Transform GetRandomPositionForVilleger()
    {
        return positionsToVillegerWalkAndSpawn[Random.Range(0, positionsToVillegerWalkAndSpawn.Count)];
    }
}

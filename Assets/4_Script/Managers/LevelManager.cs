using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager
{
    public static LevelManager Instance;

    [Header("Levels")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private int nextLevel = 0;

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
        return levels[nextLevel];
    }
    public void SetNextLevelIndex(int nextLevel)
    {
        this.nextLevel = nextLevel;
    }
    public int GetCurrentLevelIndex()
    {
        return nextLevel;
    }
    public Transform GetRandomPositionForEnemySpawn()
    {
        return positionToEnemySpawn[Random.Range(0, positionToEnemySpawn.Count)];
    }
    public Transform GetRandomPositionForVilleger()
    {
        return positionsToVillegerWalkAndSpawn[Random.Range(0, positionsToVillegerWalkAndSpawn.Count)];
    }

    public override void Destroy()
    {
        Instance = null;
    }
}

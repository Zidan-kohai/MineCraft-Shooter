using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    public static GameManager Instance;
    [Header("Level")]
    [SerializeField] private Level currentLevel;
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<Villeger> villegers;

    [Header("Parents")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform villegerParent;
    public override void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        enemies = new List<Enemy>();
        villegers = new List<Villeger>();

        SpawnHealthObjectInLevel();
    }

    public override void AfterInit()
    {
    }

    public void SpawnHealthObjectInLevel()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();

        player = Instantiate(currentLevel.playerPrefab, LevelManager.Instance.playerPositionToSpawn.position, Quaternion.identity);

        for (int i = 0; i < currentLevel.villegersCountToSpawn; i++)
        {
            villegers.Add(Instantiate(currentLevel.villegerPrefab, LevelManager.Instance.GetRandomPositionForVillegerSpawn().position, Quaternion.identity, villegerParent));
        }

        for (int i = 0; i < currentLevel.enemiesCountToSpawn; i++)
        {
            enemies.Add(Instantiate(currentLevel.enemyPrefab, LevelManager.Instance.GetRandomPositionForEnemySpawn().position, Quaternion.identity, enemyParent));
        }
    }
}

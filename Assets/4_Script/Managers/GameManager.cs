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


        EventManager.Instance.SubscribeOnDeath(RemoveHealthObjectFromList);
    }
    private void Update()
    {
        for(int i = 0; i < villegers.Count; i++)
        {
            villegers[i].EveryFrame();
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].EveryFrame();
        }
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
            villegers.Add(Instantiate(currentLevel.villegerPrefab, LevelManager.Instance.GetRandomPositionForVilleger().position, Quaternion.identity, villegerParent));
        }

        for (int i = 0; i < currentLevel.enemiesCountToSpawn; i++)
        {
            enemies.Add(Instantiate(currentLevel.enemyPrefab, LevelManager.Instance.GetRandomPositionForEnemySpawn().position, Quaternion.identity, enemyParent));
        }
    }

    public Transform GetNextPositionForVilleger()
    {
        return LevelManager.Instance.GetRandomPositionForVilleger();
    }

    public Transform GetNextVillegerForZombi(Vector3 zombiePosition)
    {
        float newVillegerDistance = Mathf.Infinity;
        Transform newVillegerTransform = null;
        foreach (var villeger in villegers)
        {
            if((villeger.transform.position - zombiePosition).magnitude < newVillegerDistance)
            {
                newVillegerDistance = (villeger.transform.position - zombiePosition).magnitude;
                newVillegerTransform = villeger.transform;
            }
        }

        return newVillegerTransform;
    }

    private void RemoveHealthObjectFromList(HealthObject healthObject)
    {
        if (healthObject is Enemy)
        {
            enemies.Remove((Enemy)healthObject);
        }
        else if(healthObject is Villeger)
        {
            villegers.Remove((Villeger)healthObject);
        }
        else
        {

        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.UnsubscribeOnDeath(RemoveHealthObjectFromList);
    }
}

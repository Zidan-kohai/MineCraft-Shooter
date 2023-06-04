using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    public static GameManager Instance;

    [Header("Level")]
    [SerializeField] private Level currentLevel;
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerRotation playerRotation;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private List<Villeger> villegers;

    [Header("Parents")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform villegerParent;

    [Header("Game Properties")]
    [SerializeField] private bool isGameStop;
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
        #region Stoping
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameStop)
        {
            isGameStop = true;

            StopGame();
        }
        if (Input.GetMouseButtonDown(0) && isGameStop)
        {
            isGameStop = false;
            ResumeGame();
        }

        if(isGameStop)
        {
            return;
        }
        #endregion

        player.EveryFrame();
        playerMovement.EveryFrame();

        for(int i = 0; i < villegers.Count; i++)
        {
            villegers[i].EveryFrame();
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].EveryFrame();
        }
    }

    public void LateUpdate()
    {
        if (isGameStop)
        {
            return;
        }

        playerRotation.AfterEveryFrame();
    }
    public override void AfterInit()
    {

    }

    public void SpawnHealthObjectInLevel()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();

        player = Instantiate(currentLevel.playerPrefab, LevelManager.Instance.playerPositionToSpawn.position, Quaternion.identity);
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRotation = player.GetComponent<PlayerRotation>();

        playerMovement.Init();
        playerRotation.Init();

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

    public HealthObject GetNextVillegerForZombi(Vector3 zombiePosition)
    {
        float newVillegerDistance = Mathf.Infinity;
        HealthObject newVillegerTransform = null;
        foreach (var villeger in villegers)
        {
            if((villeger.transform.position - zombiePosition).magnitude < newVillegerDistance)
            {
                newVillegerDistance = (villeger.transform.position - zombiePosition).magnitude;
                newVillegerTransform = villeger;
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

        if(enemies.Count == 0 || villegers.Count == 0)
        {
            NextLevel();
        }
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }
    public int GetVillegerCount()
    {
        return villegers.Count;
    }
    public void StopGame()
    {
        AudioListener.volume = 0f;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public override void Destroy()
    {
        EventManager.Instance.UnsubscribeOnDeath(RemoveHealthObjectFromList);
        Instance = null;
    }
}

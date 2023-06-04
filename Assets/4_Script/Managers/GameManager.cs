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
    [SerializeField] private bool isGameMenu = true;
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

        isGameMenu = true;
        isGameStop = true;
        StopGame();

        EventManager.Instance.SubscribeOnDeath(RemoveHealthObjectFromList);
    }
    private void Update()
    {
        #region Stoping

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameStop && !isGameMenu)
        {
            StopGame();
            EventManager.Instance.OnStopGame();
        }
        #endregion

        if (isGameStop)
        {
            return;
        }

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


        if (villegers.Count == 0)
        {
            EventManager.Instance.OnLoseGame();
            StopGame();
        }
        else if(enemies.Count == 0)
        {
            Restart();
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
        isGameStop = true;
        AudioListener.volume = 0f;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        isGameStop = false;
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetIsGameMenu(bool value)
    {
        isGameMenu = value;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public override void Destroy()
    {
        EventManager.Instance.UnsubscribeOnDeath(RemoveHealthObjectFromList);
        Instance = null;
    }
}

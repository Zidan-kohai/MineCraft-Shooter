using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
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
    [SerializeField] private int money;
    [SerializeField] private List<Mine> mines;
    [SerializeField] private Mine minePrefab;

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
        mines = new List<Mine>();

        PlayerInit();

        isGameMenu = true;
        isGameStop = true;
        StopGame();

        EventManager.Instance.SubscribeOnDeath(RemoveHealthObjectFromList);
        EventManager.Instance.SubscribeOnSetMoney(SetMoney);
        EventManager.Instance.SubscribeOnNewWave(NewWave);
    }

    public override void AfterInit()
    {
        player.AfterInit();


        if (DataManager.Instance.GetHealthObjectCount() > 0)
        {
            SpawnSavedHealthObjectInLevel();
            SpawnMine();
            EventManager.Instance.OnSetMoney(DataManager.Instance.GetCoin());

            if (enemies.Count == 0)
            {
                EventManager.Instance.OnEndWave(currentLevel.timeToNextWave);
            }

            player.SetBlowUp(DataManager.Instance.GetGranadeCount(), DataManager.Instance.GetMineInPlayerHand());
        }

        else
        {
            EventManager.Instance.OnNewWave();

            EventManager.Instance.OnSetMoney(30);
            player.SetBlowUp(7, 5);
        }


        SaveHealtObject();

        EventManager.Instance.OnStart();
    }
    
    public void AddMine(Mine mine)
    {
        mines.Add(mine);
        
        List<Vector3> position = new List<Vector3>();
        List<Quaternion> rotation = new List<Quaternion>();

        foreach (var item in mines)
        {
            position.Add(item.transform.position);
            rotation.Add(item.transform.rotation);
        }

        DataManager.Instance.SetPutMineCount(mines.Count, position, rotation);
    }

    public void RemoveMine(Mine mine)
    {
        mines.Remove(mine);

        List<Vector3> position = new List<Vector3>();
        List<Quaternion> rotation = new List<Quaternion>();

        foreach (var item in mines)
        {
            position.Add(item.transform.position);
            rotation.Add(item.transform.rotation);
        }

        DataManager.Instance.SetPutMineCount(mines.Count, position, rotation);
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

        #region ResetSave
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            DataManager.Instance.ResetSave();
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
    private void NewWave()
    {
        SpawnHealthObjectInLevel();

    }

    private void PlayerInit()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        player = Instantiate(currentLevel.playerPrefab, LevelManager.Instance.playerPositionToSpawn.position, Quaternion.identity);
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRotation = player.GetComponent<PlayerRotation>();

        player.Init();
        playerMovement.Init();
        playerRotation.Init();
    }
    public void SpawnHealthObjectInLevel()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();

        for (int i = 0; i < currentLevel.villegersCountToSpawn; i++)
        {
            Villeger villeger = Instantiate(currentLevel.villegerPrefab, LevelManager.Instance.GetRandomPositionForVilleger().position, Quaternion.identity, villegerParent);
            villegers.Add(villeger);
            villeger.Init();
        }

        for (int i = 0; i < currentLevel.simpleEnemiesCountToSpawn; i++)
        {
            Enemy enemy = Instantiate(currentLevel.simpleEnemyPrefab, LevelManager.Instance.GetRandomPositionForEnemySpawn().position, Quaternion.identity, enemyParent);
            enemies.Add(enemy);
            enemy.Init();
        }

        for (int i = 0; i < currentLevel.archerEnemiesCountToSpawn; i++)
        {
            Enemy enemy = Instantiate(currentLevel.archerEnemyPrefab, LevelManager.Instance.GetRandomPositionForEnemySpawn().position, Quaternion.identity, enemyParent);
            enemies.Add(enemy);
            enemy.Init();
        }

        for (int i = 0; i < currentLevel.creeperEnemiesCountToSpawn; i++)
        {
            Enemy enemy = Instantiate(currentLevel.creeperEnemyPrefab, LevelManager.Instance.GetRandomPositionForEnemySpawn().position, Quaternion.identity, enemyParent);
            enemies.Add(enemy);
            enemy.Init();
        }

        DataManager.Instance.SetCurrentLevel(LevelManager.Instance.GetCurrentLevelIndex());

        LevelManager.Instance.SetNextLevelIndex(LevelManager.Instance.GetCurrentLevelIndex() + 1);
    }

    public void SpawnSavedHealthObjectInLevel()
    {
        currentLevel = LevelManager.Instance.GetCurrentLevel();
        
        List<Vector3Int> position = new List<Vector3Int>();

        for(int i = 0; i < DataManager.Instance.GetVillegerCount(out position); i++)
        {
            var villeger = Instantiate(currentLevel.villegerPrefab, position[i], Quaternion.identity, villegerParent);
            villegers.Add(villeger);
            villeger.Init();
        }

        for (int i = 0; i < DataManager.Instance.GetSimpleEnemyCount(out position); i++)
        {
            var simpleEnemy = Instantiate(currentLevel.simpleEnemyPrefab, position[i], Quaternion.identity, villegerParent);
            enemies.Add(simpleEnemy);
            simpleEnemy.Init();
        }

        for (int i = 0; i < DataManager.Instance.GetArcherEnemyCount(out position); i++)
        {
            var archerEnemy = Instantiate(currentLevel.archerEnemyPrefab, position[i], Quaternion.identity, villegerParent);
            enemies.Add(archerEnemy);
            archerEnemy.Init();
        }

        for (int i = 0; i < DataManager.Instance.GetCreeperEnemyCount(out position); i++)
        {
            var creeperEnemy = Instantiate(currentLevel.creeperEnemyPrefab, position[i], Quaternion.identity, villegerParent);
            enemies.Add(creeperEnemy);
            creeperEnemy.Init();
        }

        Vector3 playerPosition = new Vector3();
        Quaternion playerRotation = new Quaternion();

        DataManager.Instance.GetPlayerTransform(ref playerPosition, ref playerRotation);

        player.transform.position = playerPosition;
        player.transform.rotation = playerRotation; 

        DataManager.Instance.SetCurrentLevel(LevelManager.Instance.GetCurrentLevelIndex());

        LevelManager.Instance.SetNextLevelIndex(LevelManager.Instance.GetCurrentLevelIndex() + 1);
    }

    public void SpawnMine()
    {
        List<Vector3> position = new List<Vector3>();
        List<Quaternion> rotation = new List<Quaternion>();

        int count = DataManager.Instance.GetPutMine(ref position, ref rotation);

        for (int i = 0; i < count; i++)
        {
            Mine mine = Instantiate(minePrefab, position[i], rotation[i]);
            mines.Add(mine);
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
            EventManager.Instance.OnEndWave(currentLevel.timeToNextWave);
        }

    }

    private void SaveHealtObject()
    {
        List<HealthObject> healthObjects = new List<HealthObject>();

        healthObjects.AddRange(villegers);
        healthObjects.AddRange(enemies);

        DataManager.Instance.SetHealtObjects(healthObjects);

        DataManager.Instance.SetPlayerTransform(player.transform.position, player.transform.rotation);

        DOTween.Sequence().AppendInterval(1).OnComplete(() =>
        {
            SaveHealtObject();
        });
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }
    public int GetVillegerCount()
    {
        return villegers.Count;
    }
    public void SetMoney(int money)
    {
        this.money = money;

        DataManager.Instance.SetCoin(money);
    }

    public int GetMoney()
    {
        return money;
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
        EventManager.Instance.UnsubscribeOnSetMoney(SetMoney);
        EventManager.Instance.UnsubscribeOnNewWave(NewWave);
        Instance = null;
    }
}

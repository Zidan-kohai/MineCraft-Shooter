using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Level Properties")]
    public int timeToNextWave;

    [Header("Simple Enemies")]
    public Enemy simpleEnemyPrefab;
    public int simpleEnemiesCountToSpawn;


    [Header("Archer Enemies")]
    public Enemy archerEnemyPrefab;
    public int archerEnemiesCountToSpawn;

    [Header("Creeper Enemies")]
    public Enemy creeperEnemyPrefab;
    public int creeperEnemiesCountToSpawn;

    [Header("Player")]
    public PlayerInteraction playerPrefab;
 
    [Header("Villager")]
    public Villeger villegerPrefab;
    public int villegersCountToSpawn;

    [Header("Yandex")]
    public bool ShowADV = false;
}

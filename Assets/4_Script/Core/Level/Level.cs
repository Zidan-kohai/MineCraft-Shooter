using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Level Properties")]
    public int timeToNextWave;

    [Header("Enemies")]
    public Enemy enemyPrefab;
    public int enemiesCountToSpawn;

    [Header("Player")]
    public PlayerInteraction playerPrefab;
 
    [Header("Villager")]
    public Villeger villegerPrefab;
    public int villegersCountToSpawn;
}

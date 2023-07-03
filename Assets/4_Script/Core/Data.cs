using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public int CurrentLevel;
    public int Coin;

    #region HealtObject

    public int VillegerCount;
    public List<Vector3Int> VillegerPositions;


    public int SimpleEnemiesCount;
    public List<Vector3Int> SimpleEnemyPositions;


    public int ArcherEnemiesCount;
    public List<Vector3Int> ArcherEnemyPositions;

    public int CreeperEnemyCount;
    public List<Vector3Int> CreeperEnemyPositions;
    #endregion

}

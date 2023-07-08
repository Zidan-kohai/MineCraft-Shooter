using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
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

    #region Weapon

    public WeaponInPlayerHand weaponInPlayerHand;

    public Vector3 GunPosition;
    public Quaternion GunRotation;

    public Vector3 MKPosition;
    public Quaternion MKRotation;

    public Vector3 ShotgunPosition;
    public Quaternion ShotgunRotation;

    public enum WeaponInPlayerHand
    {
        None,
        Gun,
        MK,
        Shotgun
    }

    #endregion

}

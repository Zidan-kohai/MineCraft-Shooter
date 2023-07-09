using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Data
{
    public int CurrentLevel;
    public int Coin;

    #region HealtObject

    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;

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
    public int allPatronInGun, currentPatronInMagazineOfGun;

    public Vector3 MKPosition;
    public Quaternion MKRotation;
    public int allPatronInMK, currentPatronInMagazineOfMK;

    public Vector3 ShotgunPosition;
    public Quaternion ShotgunRotation;
    public int allPatronInShotgun, currentPatronInMagazineOfShotgun;

    public int GranadeCount;

    public int PutMineCount;
    public List<Vector3> MinesPosition;
    public List<Quaternion> MinesRotation;

    public int mineCountInPlayerHand;
    public enum WeaponInPlayerHand
    {
        None,
        Gun,
        MK,
        Shotgun
    }

    #endregion

}

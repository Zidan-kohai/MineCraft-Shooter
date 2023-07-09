using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class DataManager : Manager
{
    public static DataManager Instance;
    private Data data;


    public override void Init()
    {
        LoadData();

        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    #region CurrentLevel
    public int GetCurrentLevel()
    {
        return data.CurrentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        data.CurrentLevel = level;
        SaveData();
    }
    #endregion

    #region Coin
    public int GetCoin()
    {
        return data.Coin;
    }

    public void SetCoin(int Coin)
    {
        data.Coin = Coin;
        SaveData();
    }

    #endregion

    #region HealtObject
    public void SetHealtObjects(List<HealthObject> healthObjects)
    {
        int VillegerCount = 0, SimpleEnemyCount = 0, ArcherEnemyCount = 0, CreeperEnemyCount = 0;
        data.VillegerPositions.Clear();
        data.SimpleEnemyPositions.Clear();
        data.ArcherEnemyPositions.Clear();
        data.CreeperEnemyPositions.Clear();

        foreach (HealthObject healthObject in healthObjects)
        {
            if(healthObject is Villeger)
            {
                VillegerCount++;
                data.VillegerPositions.Add(Vector3Int.FloorToInt(healthObject.transform.position));
            }

            else if(healthObject is SimpleEnemy)
            {
                SimpleEnemyCount++;
                data.SimpleEnemyPositions.Add(Vector3Int.FloorToInt(healthObject.transform.position));
            }

            else if(healthObject is ArcherEnemy)
            {
                ArcherEnemyCount++;
                data.ArcherEnemyPositions.Add(Vector3Int.FloorToInt(healthObject.transform.position));
            }

            else if(healthObject is Creeper)
            {
                CreeperEnemyCount++;
                data.CreeperEnemyPositions.Add(Vector3Int.FloorToInt(healthObject.transform.position));
            }
        }

        data.VillegerCount = VillegerCount;
        data.SimpleEnemiesCount = SimpleEnemyCount;
        data.ArcherEnemiesCount = ArcherEnemyCount;
        data.CreeperEnemyCount = CreeperEnemyCount;

        SaveData();
    }

    public int GetVillegerCount(out List<Vector3Int> position)
    {
        position = data.VillegerPositions;
        return data.VillegerCount;
    }

    public int GetSimpleEnemyCount(out List<Vector3Int> position)
    {
        position = data.SimpleEnemyPositions;
        return data.SimpleEnemiesCount;
    }

    public int GetArcherEnemyCount(out List<Vector3Int> position)
    {
        position = data.ArcherEnemyPositions;
        return data.ArcherEnemiesCount;
    }

    public int GetCreeperEnemyCount(out List<Vector3Int> position)
    {
        position = data.CreeperEnemyPositions;
        return data.CreeperEnemyCount;
    }

    public int GetHealthObjectCount()
    {
        return data.SimpleEnemiesCount + data.ArcherEnemiesCount + data.CreeperEnemyCount + data.VillegerCount;
    }

    #endregion

    #region Weapon

    public Data.WeaponInPlayerHand GetWeaponInPLayerHand()
    {
        return data.weaponInPlayerHand;
    }

    public void SetWeaponInPLayerHand(Weapon weapon)
    {
        if (weapon == null)
        {
            data.weaponInPlayerHand = Data.WeaponInPlayerHand.None;
            Debug.Log("None");
            return; 
        }

        else if (weapon is Gun)
        {
            data.weaponInPlayerHand = Data.WeaponInPlayerHand.Gun;
            Debug.Log("Gun");
            return;
        }
        else if (weapon is MK)
        {
            data.weaponInPlayerHand = Data.WeaponInPlayerHand.MK;
            Debug.Log("MK");
            return;
        }
        else if (weapon is Shotgun)
        {
            data.weaponInPlayerHand = Data.WeaponInPlayerHand.Shotgun;
            Debug.Log("Shotgun");
            return;
        }

    }

    public void SetWeapons(List<Weapon> weapon)
    {
        foreach(Weapon weaponItem in weapon)
        {
            if (weaponItem is Gun)
            {
                data.GunPosition = weaponItem.transform.position;
                data.GunRotation = weaponItem.transform.rotation;
            }
            else if (weaponItem is MK)
            {
                data.MKPosition = weaponItem.transform.position;
                data.MKRotation = weaponItem.transform.rotation;
            }
            else if (weaponItem is Shotgun)
            {
                data.ShotgunPosition = weaponItem.transform.position;
                data.ShotgunRotation = weaponItem.transform.rotation;
            }
        }
    }

    public void GetWeaponPositionAndRotation(ref List<Vector3> Position, ref List<Quaternion> Rotation)
    {
        Position.Add(data.GunPosition);
        Position.Add(data.MKPosition);
        Position.Add(data.ShotgunPosition);

        Rotation.Add(data.GunRotation);
        Rotation.Add(data.MKRotation);
        Rotation.Add(data.ShotgunRotation);
    }

    public void GetPatrons(Weapon weapon, ref int allPatron, ref int currentPatronInMagazine)
    {
        switch(weapon)
        {
            case Gun:

                allPatron = data.allPatronInGun;
                currentPatronInMagazine = data.currentPatronInMagazineOfGun;

                break;

            case MK:

                allPatron = data.allPatronInMK;
                currentPatronInMagazine = data.currentPatronInMagazineOfMK;

                break;

            case Shotgun:

                allPatron = data.allPatronInShotgun;
                currentPatronInMagazine = data.currentPatronInMagazineOfShotgun;

                break;

        }
    }

    public void SetPatrons(Weapon weapon, int allPatron, int currentPatronInMagazine)
    {
        switch (weapon)
        {
            case Gun:

                data.allPatronInGun = allPatron;
                data.currentPatronInMagazineOfGun = currentPatronInMagazine;

                break;

            case MK:

                data.allPatronInMK = allPatron;
                data.currentPatronInMagazineOfMK = currentPatronInMagazine;

                break;

            case Shotgun:

                data.allPatronInShotgun = allPatron;
                data.currentPatronInMagazineOfShotgun = currentPatronInMagazine;

                break;
        }

        SaveData();
    }

    public int GetPutMine(ref List<Vector3> position, ref List<Quaternion> rotation)
    {

        position = data.MinesPosition; 
        rotation = data.MinesRotation;

        return data.PutMineCount;
    }

    public int GetGranadeCount()
    {
        return data.GranadeCount;
    }

    public int GetMineInPlayerHand()
    {
        return data.mineCountInPlayerHand;
    }
    public void SetMineInPlayerHand(int count)
    {
        data.mineCountInPlayerHand = count;
        SaveData();
    }
    public void SetGranadeCount(int count)
    {
        data.GranadeCount = count;
        SaveData();
    }

    public void SetPutMineCount(int count, List<Vector3> position, List<Quaternion> Rotation)
    {
        data.MinesPosition.Clear();
        data.MinesRotation.Clear();

        data.PutMineCount = count;
        data.MinesPosition = position;
        data.MinesRotation = Rotation;

        SaveData();
    }
    #endregion

    private void LoadData()
    {
        if(!PlayerPrefs.HasKey("Data"))
        {
            data = new Data();
        }
        else
        {
            string str = PlayerPrefs.GetString("Data");

            data = JsonUtility.FromJson<Data>(str);
        }

        Debug.Log("Load: " + JsonUtility.ToJson(data));
    }

    private void SaveData()
    {
        string str = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("Data", str);

        Debug.Log("Save: " + JsonUtility.ToJson(data));
    }

    internal void ResetSave()
    {
        data = new Data { };
        SaveData();

        Debug.Log("Reset: " + JsonUtility.ToJson(data));
    }
}

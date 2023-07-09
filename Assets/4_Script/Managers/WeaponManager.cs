﻿using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : Manager
{
    public static WeaponManager Instance;

    [SerializeField] private Gun gunPrefab;
    [SerializeField] private Vector3 gunDefaultPosition;
    [SerializeField] private Vector3 gunDefaultRotation;

    [SerializeField] private MK mkPrefab;
    [SerializeField] private Vector3 mkDefaultPosition;
    [SerializeField] private Vector3 mkDefaultRotation;

    [SerializeField] private Shotgun ShotgunPrefab;
    [SerializeField] private Vector3 ShotgunDefaultPosition;
    [SerializeField] private Vector3 ShotgunDefaultRotation;


    public Gun GetGunPrefab() { return gunPrefab; }

    public MK GetMKPrefab() { return mkPrefab; }

    public Shotgun GetShotgunPrefab() { return ShotgunPrefab; }



    public override void Init()
    {
        if(Instance != null)
        {
            return;
        }
        Instance = this;


        if (DataManager.Instance.GetHealthObjectCount() > 0)
        {
            spawnSavedWeapon();
        }
        else
        {
            SpawnWeapon();
        }

        SaveWeaponDate();
    }

    private void spawnSavedWeapon()
    {
        List<Vector3> spawnPosition = new List<Vector3>();
        List<Quaternion> rotates = new List<Quaternion>();

        DataManager.Instance.GetWeaponPositionAndRotation(ref spawnPosition, ref rotates);

        gunPrefab = Instantiate(gunPrefab, spawnPosition[0], rotates[0]);
        mkPrefab = Instantiate(mkPrefab, spawnPosition[1], rotates[1]);
        ShotgunPrefab = Instantiate(ShotgunPrefab, spawnPosition[2], rotates[2]);

        int allPatron = 0, currentPatron = 0;

        DataManager.Instance.GetPatrons(gunPrefab, ref allPatron, ref currentPatron);
        gunPrefab.ChangePatron(allPatron, currentPatron);

        DataManager.Instance.GetPatrons(mkPrefab, ref allPatron, ref currentPatron);
        mkPrefab.ChangePatron(allPatron, currentPatron);

        DataManager.Instance.GetPatrons(ShotgunPrefab, ref allPatron, ref currentPatron);
        ShotgunPrefab.ChangePatron(allPatron, currentPatron);
    }

    private void SpawnWeapon()
    {
        gunPrefab = Instantiate(gunPrefab, gunDefaultPosition, Quaternion.Euler(gunDefaultRotation));
        mkPrefab = Instantiate(mkPrefab, mkDefaultPosition, Quaternion.Euler(mkDefaultRotation));
        ShotgunPrefab = Instantiate(ShotgunPrefab, ShotgunDefaultPosition, Quaternion.Euler(ShotgunDefaultRotation));
    }

    private void SaveWeaponDate()
    {
        List<Weapon> weapons = new List<Weapon>();
        weapons.Add(gunPrefab);
        weapons.Add(mkPrefab);
        weapons.Add(ShotgunPrefab);

        DataManager.Instance.SetWeapons(weapons);

        DOTween.Sequence()
            .AppendInterval(1).OnStepComplete(() =>
            {
                SaveWeaponDate();
            }).SetLink(gameObject);
    }
}

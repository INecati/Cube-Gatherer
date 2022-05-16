using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ZoneManager : MonoBehaviour
{
    [SerializeField] private CubeZone[] cubeZones;
    private int nextCubeZoneIndex = 0;
    [SerializeField] private CubeZone[] unlockableZones;
    [SerializeField] private int firstZoneUnlockCost;
    [SerializeField] private int zoneUnlockCostIncrease;

    private void Awake()
    {
        GameManager.OnGameSetup += GameManager_OnGameSetup;
        GameManager.OnGameStarted += GameManager_OnGameStarted;
    }

    private void GameManager_OnGameStarted()
    {
        foreach (var zone in cubeZones)
        {
            if (zone.IsUnlocked)
                zone.StartSpawning();
        }
        foreach (var zone in unlockableZones)
        {
            if (zone.IsUnlocked)
                zone.StartSpawning();
        }
        
    }

    private void GameManager_OnGameSetup()
    {
        SetupZones();
    }

    private void SetupZones()
    {
        
        int unlockCost = firstZoneUnlockCost;
        for (int i = 0; i < unlockableZones.Length; i++)
        {
            unlockableZones[i].unlockCost = unlockCost;
            if (i != 0)
                unlockableZones[i].previousZone = unlockableZones[i - 1];
            unlockableZones[i].SetupZone();
            unlockCost += zoneUnlockCostIncrease;
        }
        unlockableZones[2].OnZoneUnlocked += SpawnHelper;
        unlockableZones[4].OnZoneUnlocked += SpawnHelper;
        unlockableZones[6].OnZoneUnlocked += SpawnHelper;
        unlockableZones[unlockableZones.Length - 1].OnZoneUnlocked += EndGame;
    }

    private void SpawnHelper(CubeZone cubeZone)
    {
        HelperController helper = UnitManager.Instance.GetHelper();
        helper.transform.position = cubeZone.transform.position;
        helper.Zone = cubeZones[nextCubeZoneIndex];
        cubeZones[nextCubeZoneIndex].UnlockZone();
        nextCubeZoneIndex++;
        helper.storageTransform = CubeStorage.Instance.transform;
    }
    private void EndGame(CubeZone cubeZone) => GameManager.Instance.EndGame();

    private void OnDestroy()
    {
        GameManager.OnGameSetup -= GameManager_OnGameSetup;
        GameManager.OnGameStarted -= GameManager_OnGameStarted;
    }
}

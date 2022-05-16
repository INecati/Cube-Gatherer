using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeZone : MonoBehaviour
{
    [Header("CubeZone Settings")]
    [SerializeField] private int cubeCapacity;
    [SerializeField] private float cubeRespawnTime;
    //[SerializeField] private float zoneWidth;
    [field: SerializeField] public float ZoneWidth { get; private set; }
    [SerializeField] private string unlockCostStr;

    public int unlockCost;
    public CubeZone previousZone;
    [field:SerializeField] public bool IsUnlocked { get; private set; }
    [SerializeField] private bool isBuyable;
    [Header("CubeZone Animation Settings")]
    [SerializeField] private float pickingAnimTime;
    [SerializeField] private float cubeJumpForce;
    [SerializeField] private float maxDelay;
    public int CubeCount { get { return cubesOnZone.Count; } }
    public event Action<CubeZone> OnZoneUnlocked;
    [SerializeField] private TMP_Text unlockCostTxt;

    private Transform tran;
    public List<CubeBase> cubesOnZone;
    private bool isCubeSpawning;
    private void Awake()
    {
        cubesOnZone = new List<CubeBase>(cubeCapacity);
        tran = transform;
    }
    public void SetupZone()
    {
        if (unlockCostTxt != null)
        {
            unlockCostTxt.enabled = !IsUnlocked;
            unlockCostTxt.SetText(string.Format(unlockCostStr, unlockCost));
        }
    }
    public void StartSpawning() => StartCoroutine(nameof(CubeSpawning));

    private void SpawnCube()
    {
        BasicCube cube = UnitManager.Instance.GetBasicCube();
        cube.transform.position = tran.position + new Vector3(Random.Range(-ZoneWidth, ZoneWidth),1f,Random.Range(-ZoneWidth, ZoneWidth));
        cube.transform.rotation = Quaternion.identity;
        cube.cubeOwner = this;
        cube.OnSpawn();
        cubesOnZone.Add(cube);
    }
    private IEnumerator CubeSpawning()
    {
        isCubeSpawning = true;
        yield return new WaitForSeconds(cubeRespawnTime);
        while (cubesOnZone.Count < cubeCapacity) { 
            //Debug.Log($"CubeSpawning List Count:{cubesOnZone.Count} capacity:{cubeCapacity}");
            SpawnCube();
            yield return new WaitForSeconds(cubeRespawnTime);
        }
        isCubeSpawning = false;
    }
    public void UnlockZone()
    {
        if (IsUnlocked)
            return;
        IsUnlocked = true;
        unlockCost = 0;
        StartCoroutine(nameof(CubeSpawning));
        if (unlockCostTxt != null)
            unlockCostTxt.enabled = false;
        OnZoneUnlocked?.Invoke(this);
    }
    public bool TryBuyZone()
    {
        if (!isBuyable || (previousZone != null && !previousZone.IsUnlocked))
            return false;

        List<CubeBase> spendCubes = CubeStorage.Instance.GetCubes(unlockCost);
        unlockCost -= spendCubes.Count;
        float delay = 0f;
        float delayIncrease = maxDelay / spendCubes.Count;
        foreach(var cube in spendCubes)
        {
            var cubeTransform = cube.transform;
            cubeTransform.DOKill();
            Vector3 newCubePos = tran.position;
            cubeTransform.DOJump(newCubePos, cubeJumpForce, 1, pickingAnimTime).SetDelay(delay)
                .OnComplete(()=> 
                {
                    if (cube.GetType() == typeof(BasicCube))
                        UnitManager.Instance.ReturnBasicCube((BasicCube)cube);
                }); 
            cubeTransform.rotation = tran.rotation;
            delay += delayIncrease;
        }
        unlockCostTxt?.SetText(String.Format(unlockCostStr, unlockCost));
        if (unlockCost == 0)
        {
            UnlockZone();
            return true;
        }
        return false;
    }
    public void OnCubePicked(CubeBase cube)
    {
        cubesOnZone.Remove(cube);
        if (!isCubeSpawning)
            StartCoroutine(nameof(CubeSpawning));
    }
    public void FillZone()
    {
        int spawnNumber = cubeCapacity - cubesOnZone.Count;
        for(int i = 0; i < spawnNumber; i++)
        {
            SpawnCube();
        }
    }
}

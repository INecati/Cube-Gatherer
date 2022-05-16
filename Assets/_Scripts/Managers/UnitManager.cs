using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitManager : StaticInstance<UnitManager>
{
    [SerializeField] private BasicCube basicCubePrefab;
    [SerializeField] private HelperController helperPrefab;

    private ObjectPool<BasicCube> basicCubePool;
    protected override void Awake()
    {
        base.Awake();
        basicCubePool = new ObjectPool<BasicCube>(CreateBasicCube, OnGetBasicCube, OnReleaseBasicCube);
        BasicCube CreateBasicCube()
        {
            return Instantiate(basicCubePrefab);
        }
        void OnReleaseBasicCube(BasicCube basicCube)
        {
            basicCube.transform.DOKill();
            basicCube.gameObject.SetActive(false);
        }
        void OnGetBasicCube(BasicCube basicCube)
        {
            basicCube.gameObject.SetActive(true);
        }
    }
    public BasicCube GetBasicCube()
    {
        return basicCubePool.Get();
    }
    public void ReturnBasicCube(BasicCube cube)
    {
        basicCubePool.Release(cube);
    }
    public HelperController GetHelper()
    {
        return Instantiate(helperPrefab);
    }
}

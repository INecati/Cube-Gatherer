using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeStorage : StaticInstance<CubeStorage>
{
    [Header("Cube Store Anim Settings")]
    [SerializeField] private float storingAnimTime;
    [SerializeField] private float cubeJumpForce;
    [Header("Cube Spacing Settings")]
    [SerializeField] private Vector2 cubeStacking;
    [SerializeField] private Vector3 cubeSpacing;

    [Header("Storage Settings")]
    [SerializeField] private int cubeCapacity;

    public int CubeCount { get { return cubeStack.Count; } }
    private Stack<CubeBase> cubeStack;
    private Vector3 firstCubePos;

    [SerializeField] private Transform storedItemsTransform;
    protected override void Awake()
    {
        base.Awake();
        cubeStack = new Stack<CubeBase>(cubeCapacity);
        firstCubePos = storedItemsTransform.localPosition + new Vector3(-cubeSpacing.x * (cubeStacking.x-1) * 0.5f, 0, -cubeSpacing.z * (cubeStacking.y-1) * 0.5f);
    }
    public CubeBase GetCube()
    {
        if (cubeStack.Count > 0)
            return cubeStack.Pop();
        else
            return null;
    }
    public bool TryStoreCube(CubeBase cube)
    {
        if (cubeStack.Count >= cubeCapacity)
            return false;
        Transform cubeTransform = cube.transform;
        cubeTransform.DOKill();
        cubeTransform.SetParent(storedItemsTransform);
        cubeTransform.DOScale(CubeBase.defaultScale, storingAnimTime);
        Vector3 newCubePos = firstCubePos + new Vector3(cubeStack.Count % cubeStacking.x * cubeSpacing.x,
            Mathf.FloorToInt(cubeStack.Count / (cubeStacking.x * cubeStacking.y)) * cubeSpacing.y,
            Mathf.FloorToInt(cubeStack.Count / cubeStacking.x) % cubeStacking.y * cubeSpacing.z);

        cubeTransform.DOLocalJump(newCubePos, cubeJumpForce, 1, storingAnimTime);
        cubeTransform.DORotateQuaternion(storedItemsTransform.rotation, storingAnimTime);

        cubeStack.Push(cube);
        return true;
    }
    public List<CubeBase> GetCubes(int spendRequest)
    {
        int amountSpend = spendRequest > cubeStack.Count ? cubeStack.Count : spendRequest;
        List<CubeBase> cubeList = new List<CubeBase>();
        for(int i = 0; i < amountSpend; i++)
        {
            cubeList.Add(cubeStack.Pop());
        }
        return cubeList;
    }
}

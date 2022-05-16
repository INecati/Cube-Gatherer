using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class CubeGathererBase : UnitBase2
{
    [Header("Cube Gatherer Settings")]
    [SerializeField] protected int cubeCarryCapacity;

    [Header("Cube Picking Animation Settings")]
    [SerializeField] protected float pickingAnimTime = 1.2f;
    [SerializeField] protected float cubeJumpForce = 2f;
    [SerializeField] protected Vector2 cubeSpacing;
    [SerializeField] protected Transform cubeBackpackTransform;

    //protected int carriedCubeCount;
    public int CubeCount { get { return cubeStack.Count; } }
    protected Stack<CubeBase> cubeStack;
    protected bool isPickingEnabled;
    protected override void Awake()
    {
        base.Awake();
        cubeStack = new Stack<CubeBase>(cubeCarryCapacity);
    }
    protected void PickCube(CubeBase cube)
    {
        Transform cubeTransform = cube.transform;
        cubeTransform.SetParent(cubeBackpackTransform);
        cubeTransform.DOScale(CubeBase.onBackScale, pickingAnimTime);
        Vector3 newCubePos = new Vector3(-cubeSpacing.x * 0.5f + cubeStack.Count % 2 * cubeSpacing.x, cubeStack.Count / 2 * cubeSpacing.y, 0);
        cubeTransform.DOLocalJump(newCubePos, cubeJumpForce, 1, pickingAnimTime);
        cubeTransform.rotation = cubeBackpackTransform.rotation;
        cubeStack.Push(cube);
    }
}

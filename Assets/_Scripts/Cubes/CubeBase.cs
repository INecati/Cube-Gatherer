using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class CubeBase : MonoBehaviour
{
    public static Vector3 defaultScale = new Vector3(0.5f, 0.5f, 0.5f);
    public static Vector3 onBackScale = new Vector3(0.25f, 0.25f, 0.25f);
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatTime;
    [SerializeField] private float fullRotationTime;

    public CubeZone cubeOwner;
    public bool IsPicked { get; private set; } = false;

    private Transform tran;
    private new Collider collider;
    private void Awake()
    {
        tran = transform;
        collider = GetComponent<Collider>();
    }
    private void Start()
    {
        OnSpawn();
    }
    public void OnSpawn()
    {
        tran.localScale = defaultScale;
        tran.DOMove(new Vector3(tran.position.x, floatHeight, tran.position.z), floatTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        tran.DORotate(new Vector3(0, 360, 0), fullRotationTime, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        collider.enabled = true;
        IsPicked = false;
    }
    public void OnPicked()
    {
        tran.DOKill();
        collider.enabled = false;
        IsPicked = true;
        cubeOwner?.OnCubePicked(this);
    }

}

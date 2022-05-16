using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class HelperController : CubeGathererBase
{
    [Header("Helper Settings")]
    [SerializeField] private float acceleration;
    private CubeBase targetCube;
    private Transform targetCubeTransform;

    public CubeZone Zone
    {
        get { return zone; }
        set
        {
            zone = value;
            if (zone != null)
                zoneTransform = value.transform;
            else
                zoneTransform = null;
        }
    }
    [SerializeField] private CubeZone zone;
    private Transform zoneTransform;
    public Transform storageTransform;
    public HelperState State { get; private set; } = HelperState.GoingZone;

    private NavMeshAgent navMeshAgent;
    private Transform tran;
    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        tran = GetComponent<Transform>();
        navMeshAgent.speed = moveSpeed;
        navMeshAgent.angularSpeed = turnSpeed;
        navMeshAgent.acceleration = acceleration;
        if (zone != null)
            zoneTransform = zone.transform;
        
    }
    private void Start()
    {
        ChangeState(HelperState.GoingZone);
    }

    void Update()
    {
        currentSpeed = navMeshAgent.velocity.magnitude;
        switch (State)
        {
            case HelperState.GoingZone:
                if (Vector3.Distance(zoneTransform.position, tran.position) < Zone.ZoneWidth)
                    ChangeState(HelperState.Picking);
                break;
            case HelperState.Picking:
                if (targetCube == null || targetCube.IsPicked) { 
                    TargetNewCube();
                    SetDestinationToCube();
                }
                break;
            case HelperState.Storing:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube") && isPickingEnabled)
        {
            if (cubeStack.Count >= cubeCarryCapacity)
                return;
            other.GetComponent<BasicCube>().OnPicked();
            BasicCube cube = other.GetComponent<BasicCube>();
            PickCube(cube);
            if (cubeStack.Count == cubeCarryCapacity)
                ChangeState(HelperState.Storing);
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("CubeStorage"))
        {
            CubeStorage storage = other.GetComponent<CubeStorage>();
            int count = cubeStack.Count;
            for (int i = 0; i < count; i++)
            {
                CubeBase cube = cubeStack.Pop();
                if (!storage.TryStoreCube(cube))
                {
                    cubeStack.Push(cube);
                    break;
                }

            }
            if (cubeStack.Count == 0)
                ChangeState(HelperState.GoingZone);
        }
    }
    private void ChangeState(HelperState newState)
    {
        State = newState;
        switch (State)
        {
            case HelperState.GoingZone:
                isPickingEnabled = false;
                navMeshAgent.SetDestination(zoneTransform.position);
                break;
            case HelperState.Picking:
                isPickingEnabled = true;
                TargetNewCube();
                SetDestinationToCube();
                break;
            case HelperState.Storing:
                navMeshAgent.SetDestination(storageTransform.position);
                break;
        }
        Debug.Log("Helper State: " + State.ToString());
    }
    private void TargetNewCube()
    {
        if (Zone.cubesOnZone.Count > 0) { 
            targetCube = Zone.cubesOnZone[0];
            targetCubeTransform = targetCube.transform;
        }
    }
    private void SetDestinationToCube()
    {
        if (targetCubeTransform != null) {
            navMeshAgent.SetDestination(targetCubeTransform.position);
        }
        else
        {
            navMeshAgent.SetDestination(tran.position);
        }

    }
}
public enum HelperState
{
    GoingZone = 0,
    Picking = 1,
    Storing = 2
}

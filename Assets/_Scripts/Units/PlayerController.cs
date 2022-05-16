using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CubeGathererBase
{
    [Header("Character Settings")]
    [Header("References")]
    [SerializeField] private Joystick Joystick;
    private Transform trans;
    private Rigidbody rb;
    protected override void Awake()
    {
        base.Awake();
        trans = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Cube"))
        {
            if (cubeStack.Count >= cubeCarryCapacity)
                return;
            other.GetComponent<BasicCube>().OnPicked();
            BasicCube cube = other.GetComponent<BasicCube>();
            PickCube(cube);
        }
        else if (other.CompareTag("CubeStorage"))
        {
            CubeStorage storage = other.GetComponent<CubeStorage>();
            int count = cubeStack.Count;
            for(int i = 0; i < count; i++)
            {
                CubeBase cube = cubeStack.Pop();
                if (!storage.TryStoreCube(cube))
                {
                    cubeStack.Push(cube);
                    break;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CubeZone"))
        {
            CubeZone zone = other.GetComponent<CubeZone>();
            if (!zone.IsUnlocked)
            {
                zone.TryBuyZone();
            }
        }

    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        float verticalDir = Joystick.Vertical;
        float horizontalDir = Joystick.Horizontal;

        Vector3 translate = new Vector3(horizontalDir, 0, verticalDir);

        translate = moveSpeed * translate;
        currentSpeed = translate.magnitude;
        translate *= Time.fixedDeltaTime;
        rb.MovePosition(translate + trans.position);

        if (currentSpeed > 0.05f)
            transform.rotation = Quaternion.LookRotation(translate, Vector3.up);
    }
}

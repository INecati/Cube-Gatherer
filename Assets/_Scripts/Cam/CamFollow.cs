using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    private Transform tran;
    void Start()
    {
        tran = transform;
    }
    private void LateUpdate()
    {
        tran.position = player.position + offset;
    }
}

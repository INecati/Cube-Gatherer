using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase2 : MonoBehaviour
{
    [Header("Unit Settings")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float turnSpeed;
    public float currentSpeed { get; protected set; }
    protected virtual void Awake()
    {
        
    }
}

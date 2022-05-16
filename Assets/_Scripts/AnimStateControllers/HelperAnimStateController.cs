using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperAnimStateController : MonoBehaviour
{
    [SerializeField] private HelperController helperController;
    private Animator animator;

    private int speedHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
    }
    void Update()
    {
        animator.SetFloat(speedHash, helperController.currentSpeed);
    }
}

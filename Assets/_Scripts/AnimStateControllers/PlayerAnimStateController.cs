using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimStateController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Animator animator;
    private int speedHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
    }
    void Update()
    {
        animator.SetFloat(speedHash, playerController.currentSpeed);
    }
}

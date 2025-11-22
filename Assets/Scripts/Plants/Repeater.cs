using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : PeaShooter
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetSingleShotBulletQuantity(2);
    }

    public override void Boost()
    {
        base.Boost();
        animator.SetTrigger("Boost");
    }
}

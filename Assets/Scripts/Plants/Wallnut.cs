using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallnut : Plant
{
    private Animator animator;

    private const string HealthPercentageString = "HealthPercentage";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetFloat(HealthPercentageString, 1f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        animator.SetFloat(HealthPercentageString, GetHealthPercentage());
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        animator.SetFloat(HealthPercentageString, GetHealthPercentage());
    }

    public override void Boost()
    {
        if (hasBoosted) return;

        maxHealth *= 2;
        FullHeal();
        animator.SetTrigger("Boosted");
        BoostComplete();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallnut : Plant
{
    private Animator animator;

    private const string m_HealthPercentageParameterString = "HealthPercentage";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetFloat(m_HealthPercentageParameterString, 1f);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        animator.SetFloat(m_HealthPercentageParameterString, GetHealthPercentage());
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        animator.SetFloat(m_HealthPercentageParameterString, GetHealthPercentage());
    }

    public override void Boost()
    {
        if (hasBoosted) return;

        maxHealth *= 2;
        FullHeal();
        BoostComplete();
    }
}

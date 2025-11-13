using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CherryBomb : Plant
{
    private Animator animator;

    public GameObject explodeEffectPrefab;

    [SerializeField] private float explosionRadius;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SwitchToDisabled();
    }

    public void Explode()
    {
        Instantiate(explodeEffectPrefab, transform.position, Quaternion.identity);
        BombardZombies();
        Destruct();
    }

    private void BombardZombies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                Zombie zombie = collider.GetComponent<Zombie>();
                if (zombie != null)
                {
                    zombie.BoomDie();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void Boost()
    {
        if (hasBoosted) return;

        explosionRadius *= 1.5f;
        BoostComplete();
    }
}

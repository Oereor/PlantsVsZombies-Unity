using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PotatoMineState
{
    Inactive,
    Active,
    Exploded
}

public class PotatoMine : Plant
{
    private PotatoMineState currentPotatoMineState = PotatoMineState.Inactive;

    [SerializeField] private float activationTime;
    private float activationTimer = 0f;

    public GameObject inactiveCustom;
    public GameObject activeCustom;
    public GameObject explodedCustom;

    [SerializeField] private float explosionRadius;

    private void Start()
    {
        SwitchToDisabled();
        SwitchToInactive();
    }

    private void Update()
    {
        switch (currentPotatoMineState)
        {
            case PotatoMineState.Inactive:
                InactiveUpdate();
                break;
            case PotatoMineState.Active:
                ActiveUpdate();
                break;
            case PotatoMineState.Exploded:
                ExplodedUpdate();
                break;
            default:
                break;
        }
    }

    private void SwitchToActive()
    {
        currentPotatoMineState = PotatoMineState.Active;

        inactiveCustom.SetActive(false);
        activeCustom.SetActive(true);
        explodedCustom.SetActive(false);
    }

    private void SwitchToInactive()
    {
        currentPotatoMineState = PotatoMineState.Inactive;

        inactiveCustom.SetActive(true);
        activeCustom.SetActive(false);
        explodedCustom.SetActive(false);
    }

    private void SwitchToExploded()
    {
        currentPotatoMineState = PotatoMineState.Exploded;

        inactiveCustom.SetActive(false);
        activeCustom.SetActive(false);
        explodedCustom.SetActive(true);

        StartCoroutine(VanishAfterExplosion());
    }

    IEnumerator VanishAfterExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destruct();
    }

    private void InactiveUpdate()
    {
        if (currentState != PlantState.Planted)
        {
            return;
        }
        activationTimer += Time.deltaTime;
        if (activationTimer >= activationTime)
        {
            SwitchToActive();
        }
    }

    protected override void SwitchToDisabled()
    {
        base.SwitchToDisabled();
        SwitchToInactive();
    }

    public override void SwitchToPlanted()
    {
        base.SwitchToPlanted();
        SwitchToInactive();
    }

    private void ActiveUpdate()
    {

    }

    private void ExplodedUpdate()
    {

    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        SwitchToExploded();
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                Zombie zombie = collider.GetComponent<Zombie>();
                if (zombie != null && (hasBoosted || zombie.RowIndex == rowIndex)) // Only affect zombies in the same row unless boosted
                {
                    zombie.BoomDie();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentPotatoMineState == PotatoMineState.Active && collision.CompareTag("Zombie"))
        {
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (currentPotatoMineState == PotatoMineState.Active && collision.CompareTag("Zombie"))
        {
            Explode();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public bool CanTakeDamage()
    {
        return currentPotatoMineState == PotatoMineState.Inactive;
    }

    public override void Boost()
    {
        if (hasBoosted)
        {
            BoostComplete();
            return;
        }

        SwitchToActive();
        explosionRadius *= 1.2f;
        BoostComplete();
    }
}

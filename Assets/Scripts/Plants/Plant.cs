using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlantState
{
    Disabled,
    Planted
}

public class Plant : MonoBehaviour
{
    protected PlantState currentState = PlantState.Disabled;

    public PlantType plantType;

    [SerializeField] private int plantHealth;

    [SerializeField] private int maxHealth;

    [SerializeField] protected int sunPaybackQuantity;

    protected bool hasBoosted = false;
    public bool HasBoosted
    {
        get { return hasBoosted; }
    }

    protected int rowIndex;
    public int RowIndex
    {
        set { rowIndex = value; }
    }

    private void Start()
    {
        SwitchToDisabled();
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlantState.Disabled:
                DisabledUpdate();
                break;
            case PlantState.Planted:
                PlantedUpdate();
                break;
            default:
                break;
        }
    }

    protected virtual void SwitchToDisabled()
    {
        currentState = PlantState.Disabled;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            GetComponent<Animator>().enabled = false;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public virtual void SwitchToPlanted()
    {
        currentState = PlantState.Planted;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            GetComponent<Animator>().enabled = true;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        OnPlanted?.Invoke();
    }

    protected virtual void PlantedUpdate()
    {

    }

    void DisabledUpdate()
    {
    }

    public void Heal(int healAmount)
    {
        plantHealth += healAmount;
        if (plantHealth > maxHealth)
        {
            plantHealth = maxHealth;
        }
    }

    public void FullHeal()
    {
        plantHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        plantHealth -= damage;
        if (plantHealth <= 0)
        {
            Destruct();
        }
    }

    public void ShovelDestruct()
    {
        plantHealth = 0;
        SunManager.Instance.ProduceSunAt(transform.position, sunPaybackQuantity);
        Destruct();
    }

    protected void Destruct()
    {
        OnDestructed?.Invoke();
        Destroy(gameObject);
    }

    public virtual void Boost()
    {

    }

    protected void BoostComplete()
    {
        hasBoosted = true;
        FullHeal();
        OnBoostCompleted?.Invoke();
    }

    public event UnityAction OnPlanted;

    public event UnityAction OnDestructed;

    public event UnityAction OnBoostCompleted;
}

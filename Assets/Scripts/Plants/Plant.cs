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

    [SerializeField] protected PlantType plantType;

    [SerializeField] protected int plantHealth;

    [SerializeField] protected int maxHealth;

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
        plantHealth = maxHealth;
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
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && collider.enabled)
        {
            collider.enabled = false;
        }
    }

    public virtual void Heal(int healAmount)
    {
        plantHealth += healAmount;
        if (plantHealth > maxHealth)
        {
            plantHealth = maxHealth;
        }
    }

    public void FullHeal()
    {
        if (plantHealth < maxHealth)
        {
            Heal(maxHealth - plantHealth);
        }
    }

    public virtual void TakeDamage(int damage)
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

    public void Destruct()
    {
        OnDestructed?.Invoke();
        Destroy(gameObject);
    }

    public PlantType GetPlantType()
    {
        return plantType;
    }

    public virtual void Boost()
    {
        return;
    }

    protected void BoostComplete()
    {
        hasBoosted = true;
        FullHeal();
        OnBoostCompleted?.Invoke();
    }

    protected float GetHealthPercentage()
    {
        return (float)plantHealth / maxHealth;
    }

    public event UnityAction OnPlanted;

    public event UnityAction OnDestructed;

    public event UnityAction OnBoostCompleted;
}

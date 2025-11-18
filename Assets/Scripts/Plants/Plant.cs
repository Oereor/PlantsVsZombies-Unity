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

    [SerializeField] protected float plantHealth;

    [SerializeField] protected float maxHealth;

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
            animator.enabled = false;
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
            animator.enabled = true;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        OnPlanted?.Invoke();
    }

    /// <summary>
    /// Performs an update operation for a planted entity. Derived classes can override this method to implement custom
    /// update logic.
    /// </summary>
    /// <remarks>This method is called during the update cycle for planted entities. The base implementation
    /// does not perform any actions.</remarks>
    protected virtual void PlantedUpdate()
    {
        return;
    }

    void DisabledUpdate()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && collider.enabled)
        {
            collider.enabled = false;
        }
    }

    public virtual void Heal(float healAmount)
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

    public virtual void TakeDamage(float damage)
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

    /// <summary>
    /// Performs a boost operation on the current instance. The specific effect of boosting depends on the derived class
    /// implementation.
    /// </summary>
    /// <remarks>Override this method in a derived class to provide custom boost behavior. The base
    /// implementation does not perform any action.</remarks>
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
        return plantHealth / maxHealth;
    }

    public event UnityAction OnPlanted;

    public event UnityAction OnDestructed;

    public event UnityAction OnBoostCompleted;
}

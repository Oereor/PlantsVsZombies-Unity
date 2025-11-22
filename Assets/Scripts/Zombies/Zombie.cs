using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

enum ZombieState
{
    Walking,
    Attacking,
    Dead
}

public class Zombie : MonoBehaviour
{
    private const string m_HealthPercentageParameterName = "HealthPercentage";

    private Rigidbody2D rb;

    private float moveSpeed = 1f;
    private readonly float attackDamage = 5f;
    private readonly float attackInterval = 0.2f;
    private float attackTimer = 0;

    private Animator animator;

    private ZombieState currentZombieState = ZombieState.Walking;

    private Plant currentTargetPlant = null;

    private float health;
    private float maxHealth = 100;

    private int rowIndex;
    public int RowIndex
    {
        get { return rowIndex; }
        set { rowIndex = value; }
    }

    private void Awake()
    {
        health = maxHealth;
    }

    private void Start()
    {
        AcquireComponents();
    }

    protected void AcquireComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentZombieState)
        {
            case ZombieState.Walking:
                WalkingUpdate();
                break;
            case ZombieState.Attacking:
                AttakingUpdate();
                break;
            case ZombieState.Dead:
                break;
            default:
                break;
        }
    }

    private void WalkingUpdate()
    {
        float randomSpeed = Random.Range(-0.1f, 0.3f) + moveSpeed;
        rb.MovePosition(rb.position + randomSpeed * Time.deltaTime * Vector2.left);
    }

    private void AttakingUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackInterval && currentTargetPlant != null)
        {
            currentTargetPlant.TakeDamage(attackDamage);
            attackTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant"))
        {
            animator.SetBool("IsAttacking", true);
            SwitchToAttacking();
            currentTargetPlant = collision.GetComponent<Plant>();
        }
        else if (collision.CompareTag("PotatoMine"))
        {
            PotatoMine potatoMine = collision.GetComponent<PotatoMine>();
            if (potatoMine != null && potatoMine.CanTakeDamage())
            {
                animator.SetBool("IsAttacking", true);
                SwitchToAttacking();
                currentTargetPlant = collision.GetComponent<PotatoMine>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant") || collision.CompareTag("PotatoMine"))
        {
            if (currentZombieState != ZombieState.Attacking)
            {
                return;
            }
            animator.SetBool("IsAttacking", false);
            currentZombieState = ZombieState.Walking;
            currentTargetPlant = null;
        }
    }

    private void SwitchToAttacking()
    {
        currentZombieState = ZombieState.Attacking;
        attackTimer = 0;
    }

    private void SwitchToDead()
    {
        currentZombieState = ZombieState.Dead;
        rb.velocity = Vector2.zero;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        OnZombieDie?.Invoke(this);
    }

    public void OnZombieBoomDead()
    {
        Destroy(gameObject, 0.5f);
    }

    public void OnZombieLostHeadDead()
    {
        Destroy(gameObject, 0.25f);
    }

    public void OnZombieBooming()
    {
        RandomlyDropSun();
        SwitchToDead();
    }

    protected void SetMaxHealth(float maxHealthValue)
    {
        maxHealth = maxHealthValue;
        health = maxHealth;
        UpdateHealthPercentage();
    }

    protected void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void RandomlyDropSun()
    {
        if (Random.Range(0, 100) < 15) // 15% chance to drop sun
        {
            SunManager.Instance.ProduceSunAt(transform.position);
        }
    }

    public void BoomDie()
    {
        animator.SetTrigger("HasBoomed");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthPercentage();
        if (health <= 0 && currentZombieState != ZombieState.Dead)
        {
            SwitchToDead();
        }
    }

    private void UpdateHealthPercentage()
    {
        float healthPercentage = health / maxHealth;
        animator.SetFloat(m_HealthPercentageParameterName, healthPercentage);
    }

    public event UnityAction<Zombie> OnZombieDie;
}

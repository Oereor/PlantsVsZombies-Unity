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
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float baselineSpeed;
    private float baseAttackDamage = 5f;
    private float moveSpeed;
    private float attackDamage = 5f;
    private readonly float attackInterval = 0.2f;
    private float attackTimer = 0;

    private float slowDownDuration = 5f;
    private float slowDownTimer = 0f;
    private bool isSlowedDown = false;

    private Animator animator;

    private ZombieState currentZombieState = ZombieState.Walking;

    private Plant currentTargetPlant = null;

    private float health;
    [SerializeField] private float maxHealth;

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
        InitializeComponents();
    }

    protected void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SetSpeed(baselineSpeed + Random.Range(-0.2f, 0.8f));
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
        if (isSlowedDown)
        {
            slowDownTimer += Time.deltaTime;
            if (slowDownTimer >= slowDownDuration)
            {
                ExitSlowDownState();
            }
        }
    }

    public void SlowDown()
    {
        if (!isSlowedDown)
        {
            EnterSlowDownState();
        }
        slowDownTimer = 0f; // Reset timer if already slowed down
    }

    private void EnterSlowDownState()
    {
        spriteRenderer.color = Color.cyan;
        isSlowedDown = true;
        slowDownTimer = 0f;
        SetSpeed(baselineSpeed * 0.75f);
        attackDamage = baseAttackDamage * 0.75f;
    }

    private void ExitSlowDownState()
    {
        spriteRenderer.color = Color.white;
        isSlowedDown = false;
        SetSpeed(baselineSpeed);
        attackDamage = baseAttackDamage;
    }

    private void WalkingUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * Vector2.left);
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
        ExitSlowDownState();
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

    public void OnHitByPea(float damage)
    {
        TakeDamage(damage);
    }

    private void UpdateHealthPercentage()
    {
        float healthPercentage = health / maxHealth;
        animator.SetFloat(m_HealthPercentageParameterName, healthPercentage);
    }

    public event UnityAction<Zombie> OnZombieDie;
}

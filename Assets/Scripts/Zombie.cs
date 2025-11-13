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
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackInterval;
    private float attackTimer = 0;

    private Animator animator;

    private ZombieState currentZombieState = ZombieState.Walking;

    private Plant currentTargetPlant = null;

    private int rowIndex;
    public int RowIndex
    {
        get { return rowIndex; }
        set { rowIndex = value; }
    }

    private void Start()
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
        float randomSpeed = Random.Range(-0.1f, 0.5f) + moveSpeed;
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
        OnZombieDie?.Invoke(this);
    }

    public void OnZombieBoomDead()
    {
        Destroy(gameObject, 0.5f);
    }

    public void OnZombieBooming()
    {
        SwitchToDead();
    }

    public void BoomDie()
    {
        animator.SetTrigger("HasBoomed");
    }

    public event UnityAction<Zombie> OnZombieDie;
}

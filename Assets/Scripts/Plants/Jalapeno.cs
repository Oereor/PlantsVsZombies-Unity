using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{
    private Animator animator;

    public GameObject firePrefab;

    private float[] rowBaseCoordinates = { 4.95f, 3.32f, 1.61f, 0f, -1.5876f };

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
        if (hasBoosted)
        {
            BoostedBurnZombies();
        }
        else
        {
            Vector3 firePosition = new Vector3(0.7f, transform.position.y + 0.3f, transform.position.z);
            Instantiate(firePrefab, firePosition, Quaternion.identity);
            BurnZombies();
        }
        Destruct();
    }

    private void BurnZombies()
    {
        Zombie[] targetZombies = ZombieManager.Instance.GetZombiesInRow(rowIndex);
        foreach (Zombie zombie in targetZombies)
        {
            zombie.BoomDie();
        }
    }

    private void BoostedBurnZombies()
    {
        for (int i = rowIndex - 2; i <= rowIndex; i++)
        {
            if (i >= 0 && i <= 4)
            {
                Vector3 firePos = new Vector3(0.7f, rowBaseCoordinates[i] - 1.7f, transform.position.z);
                Instantiate(firePrefab, firePos, Quaternion.identity);
                BurnZombies(i + 1);
            }
        }
    }

    private void BurnZombies(int row)
    {
        Zombie[] targetZombies = ZombieManager.Instance.GetZombiesInRow(row);
        foreach (Zombie zombie in targetZombies)
        {
            zombie.BoomDie();
        }
    }

    public override void Boost()
    {
        if (hasBoosted) return;

        BoostComplete();
    }
}

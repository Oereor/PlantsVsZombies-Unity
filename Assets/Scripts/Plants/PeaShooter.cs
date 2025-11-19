using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    private static readonly WaitForSeconds m_waitForSeconds0_1 = new WaitForSeconds(0.1f);
    
    [SerializeField] private float shootInterval;
    private float shootTimer = 0;

    [SerializeField] private Transform peaSpawnPoint;

    [SerializeField] private PeaBullet peaBulletPrefab;

    private readonly float bulletSpeed = 5f;

    private int singleShotQuantity = 1;

    protected override void PlantedUpdate()
    {
        if (!ZombieManager.Instance.ExistZombiesInRow(rowIndex))
        {
            return;
        }
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            StartCoroutine(Shoot(singleShotQuantity));
            shootTimer = 0;
        }
    }

    public override void Boost()
    {
        if (hasBoosted) return;
        StartCoroutine(Shoot(bulletQuantity: 40));
        singleShotQuantity *= 2;
        shootInterval *= 0.8f;
        BoostComplete();
    }

    IEnumerator Shoot(int bulletQuantity)
    {
        for (int i = 0; i < bulletQuantity; i++)
        {
            PeaBullet peaBullet = Instantiate(peaBulletPrefab, peaSpawnPoint.position, Quaternion.identity);
            peaBullet.SetSpeed(bulletSpeed);

            yield return m_waitForSeconds0_1;
        }
    }
}

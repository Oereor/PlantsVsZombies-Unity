using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    private static readonly WaitForSeconds m_waitForSeconds0_1 = new WaitForSeconds(0.1f);

    private float shootInterval = 1.5f;
    private float shootTimer = 0;

    [SerializeField] private Transform peaSpawnPoint;

    [SerializeField] private PeaBullet peaBulletPrefab;

    private float bulletSpeed = 5f;

    private float bulletDamage = 10f;

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
        StartCoroutine(Shoot(bulletQuantity: singleShotQuantity * 35));
        singleShotQuantity *= 2;
        shootInterval *= 0.8f;
        bulletDamage *= 1.2f;
        bulletSpeed *= 1.5f;
        BoostComplete();
    }

    IEnumerator Shoot(int bulletQuantity)
    {
        for (int i = 0; i < bulletQuantity; i++)
        {
            PeaBullet peaBullet = Instantiate(peaBulletPrefab, peaSpawnPoint.position, Quaternion.identity);
            peaBullet.SetSpeed(bulletSpeed);
            peaBullet.SetDamage(bulletDamage);

            yield return m_waitForSeconds0_1;
        }
    }

    protected void SetSingleShotBulletQuantity(int quantity)
    {
        singleShotQuantity = quantity;
    }
}

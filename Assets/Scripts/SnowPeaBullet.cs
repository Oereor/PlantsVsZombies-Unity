using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPeaBullet : PeaBullet
{
    protected override void OnHitZombie(Zombie zombie)
    {
        base.OnHitZombie(zombie);
        if (zombie != null)
        {
            zombie.SlowDown();
        }
    }
}

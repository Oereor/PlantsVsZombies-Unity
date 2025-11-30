using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPea : PeaShooter
{
    public override void Boost()
    {
        Zombie[] zombiesInRow = ZombieManager.Instance.GetZombiesInRow(rowIndex);
        foreach (Zombie zombie in zombiesInRow)
        {
            zombie.SlowDown();
        }
        base.Boost();
    }
}

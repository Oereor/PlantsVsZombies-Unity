using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketheadZombie : Zombie
{
    private void Start()
    {
        AcquireComponents();
        SetMaxHealth(800f);
        SetSpeed(1.2f);
    }
}

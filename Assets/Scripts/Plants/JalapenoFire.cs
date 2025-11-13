using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JalapenoFire : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnFireVanished()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBombExplosionEffect : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnExplosionEffectFinished()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sunflower : Plant
{
    [SerializeField] private float sunProductionInterval;
    private float sunProductionTimer = 0;

    private Animator animator;

    public GameObject sunPrefab;

    private readonly float minHorizontalOffset = 0.3f;
    private readonly float maxHorizontalOffset = 1.3f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ProduceSun()
    {
        GameObject newSun = Instantiate(sunPrefab, transform.position, Quaternion.identity);

        float offset = Random.Range(minHorizontalOffset, maxHorizontalOffset);
        offset *= Random.Range(0, 2) == 0 ? -1 : 1; // Randomly decide left or right

        newSun.GetComponent<Sun>().JumpTo(new Vector3(transform.position.x + offset, transform.position.y, transform.position.z));
    }

    protected override void PlantedUpdate()
    {
        sunProductionTimer += Time.deltaTime;

        if (sunProductionTimer >= sunProductionInterval)
        {
            animator.SetTrigger("IsGlowing");
            sunProductionTimer = 0;
        }
    }

    public override void Boost()
    {
        if (hasBoosted) return;

        animator.SetTrigger("IsGlowing");
        sunProductionTimer = 0;
        for (int i = 0; i < 4; i++)
        {
            ProduceSun();
        }
        sunProductionInterval *= 0.75f;
        BoostComplete();
    }
}

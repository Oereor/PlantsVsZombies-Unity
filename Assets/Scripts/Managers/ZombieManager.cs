using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SpawnState
{
    NotStarted,
    Spawning,
    Completed
}

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }

    public Transform[] spawnPoints;
    public GameObject zombiePrefab;

    private SpawnState currentSpawnState = SpawnState.NotStarted;

    private readonly List<Zombie> activeZombies = new List<Zombie>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        currentSpawnState = SpawnState.Spawning;
        StartCoroutine(SpawnZombie(waves: 8));
    }

    IEnumerator SpawnZombie(int waves)
    {
        for (int i = 0; i <= waves; i++)
        {
            StartCoroutine(SpawnSingleWave(zombieQuantity: 10 * i));
            yield return new WaitForSeconds(20 * i);
        }
    }

    private IEnumerator SpawnSingleWave(int zombieQuantity)
    {
        int spawnedZombies = 0;
        while (currentSpawnState == SpawnState.Spawning && spawnedZombies <= zombieQuantity)
        {
            float randomDelay = Random.Range(0.5f, 2f);
            SpawnSingleZombie();
            spawnedZombies++;
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private void SpawnSingleZombie()
    {
        int rowIndex = Random.Range(0, spawnPoints.Length);
        GameObject newZombie = Instantiate(zombiePrefab, spawnPoints[rowIndex].position, Quaternion.identity);
        Zombie zombieComponent = newZombie.GetComponent<Zombie>();
        zombieComponent.RowIndex = rowIndex + 1;
        zombieComponent.OnZombieDie += RemoveDeadZombie;
        activeZombies.Add(zombieComponent);
    }

    public Zombie[] GetZombiesInRow(int rowIndex)
    {
        List<Zombie> zombiesInRow = new List<Zombie>();
        foreach (Zombie zombie in activeZombies)
        {
            if (zombie.RowIndex == rowIndex)
            {
                zombiesInRow.Add(zombie);
            }
        }
        return zombiesInRow.ToArray();
    }

    private void RemoveDeadZombie(Zombie deadZombie)
    {
        if (activeZombies.Contains(deadZombie))
        {
            activeZombies.Remove(deadZombie);
        }
    }

    public bool ExistZombiesInRow(int rowIndex)
    {
        foreach (Zombie zombie in activeZombies)
        {
            if (zombie.RowIndex == rowIndex)
            {
                return true;
            }
        }
        return false;
    }
}

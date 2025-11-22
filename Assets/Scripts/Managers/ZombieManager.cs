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
    public GameObject[] zombiePrefabs;

    private SpawnState currentSpawnState = SpawnState.NotStarted;

    private readonly List<Zombie> activeZombies = new List<Zombie>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStartPlanting += StartSpawning;
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
        GameObject newZombie = Instantiate(zombiePrefabs[GetRandomZombieType()], spawnPoints[rowIndex].position, Quaternion.identity);
        Zombie zombieComponent = newZombie.GetComponent<Zombie>();
        zombieComponent.RowIndex = rowIndex + 1;
        zombieComponent.OnZombieDie += RemoveDeadZombie;
        activeZombies.Add(zombieComponent);
    }

    /// <summary>
    /// Selects a random zombie type identifier based on predefined probability distribution.
    /// </summary>
    /// <remarks>The probability distribution is weighted: Ordinary zombies are selected most frequently,
    /// followed by Conehead and then Buckethead zombies. Use the returned value to determine which zombie type to spawn
    /// or display.</remarks>
    /// <returns>An integer representing the zombie type: 0 for Ordinary, 1 for Conehead, or 2 for Buckethead.</returns>
    private int GetRandomZombieType()
    {
        int randomValue = Random.Range(0, 100);
        if (randomValue < 60)
        {
            return 0; // Ordinary
        }
        else if (randomValue < 85)
        {
            return 1; // Conehead
        }
        else
        {
            return 2; // Buckethead
        }
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

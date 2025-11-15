using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public Plant[] plantPrefabs;

    public Shovel shovelPrefab;

    public BoostSun boostSunPrefab;

    private Plant currentPlantInHand;

    private Shovel shovelInHand;

    private BoostSun boostSunInHand;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (currentPlantInHand != null)
        {
            FollowMouseCursor(currentPlantInHand.transform);
        }
        if (shovelInHand != null)
        {
            FollowMouseCursor(shovelInHand.transform, 0.5f, 0.5f);
        }
        if (boostSunInHand != null)
        {
            FollowMouseCursor(boostSunInHand.transform);
        }
    }

    // returns the grabbed plant
    public Plant GrabOrDisposePlant(PlantType plantType)
    {
        if (currentPlantInHand != null)
        {
            if (currentPlantInHand.GetPlantType() == plantType)
            {
                currentPlantInHand.Destruct();
                currentPlantInHand = null;
            }
            return null;
        }

        Plant plantPrefab = GetPlantPrefab(plantType);
        if (plantPrefab == null)
        {
            print("No prefab found for plant type: " + plantType);
            return null;
        }

        currentPlantInHand = Instantiate(plantPrefab);
        return currentPlantInHand;
    }

    public void GrabOrDisposeShovel()
    {
        if (shovelInHand != null)
        {
            shovelInHand.Destruct();
            shovelInHand = null;
            return;
        }

        shovelInHand = Instantiate(shovelPrefab);
    }

    public void GrabOrDisposeBoostSun()
    {
        if (boostSunInHand != null)
        {
            boostSunInHand.Destruct();
            boostSunInHand = null;
            return;
        }

        boostSunInHand = Instantiate(boostSunPrefab);
    }

    private Plant GetPlantPrefab(PlantType plantType)
    {
        foreach (Plant plant in plantPrefabs)
        {
            if (plant.GetPlantType() == plantType)
            {
                return plant;
            }
        }
        return null;
    }

    private void FollowMouseCursor(Transform target, float xOffset = 0, float yOffset = 0)
    {
        if (target == null) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.x += xOffset;
        mouseWorldPosition.y += yOffset;
        mouseWorldPosition.z = 0;
        target.position = mouseWorldPosition;
    }

    public void OnCellClick(Cell cell)
    {
        if (currentPlantInHand != null)
        {
            DropPlant(cell);
            return;
        }
        if (shovelInHand != null)
        {
            RemovePlant(cell);
            return;
        }
        if (boostSunInHand != null)
        {
            BoostCell(cell);
            return;
        }
    }

    private void DropPlant(Cell cell)
    {
        bool dropSuccess = cell.TryDropPlant(currentPlantInHand);
        if (dropSuccess)
        {
            currentPlantInHand = null;
        }
    }

    private void RemovePlant(Cell cell)
    {
        bool removeSuccess = cell.TryRemovePlant();
        if (removeSuccess)
        {
            shovelInHand.Destruct();
            shovelInHand = null;
        }
    }

    private void BoostCell(Cell cell)
    {
        bool boostSuccess = cell.TryBoost(boostSunInHand);
        if (boostSuccess)
        {
            SunManager.Instance.ConsumeSun(BoostSun.BoostCost);
            boostSunInHand = null;
        }
    }
}

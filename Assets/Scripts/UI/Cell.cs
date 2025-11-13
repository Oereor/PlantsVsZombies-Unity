using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    public Plant currentPlant;

    [SerializeField] private int rowIndex;

    private bool isBoosted = false;

    private BoostSun currentBoostSun;

    private void OnMouseDown()
    {
        HandManager.Instance.OnCellClick(this);
    }

    public bool TryDropPlant(Plant plant)
    {
        if (currentPlant != null) return false;

        currentPlant = plant;
        Vector3 plantPosition = transform.position;
        plantPosition.z = 0;
        currentPlant.transform.position = plantPosition;
        currentPlant.SwitchToPlanted();
        currentPlant.RowIndex = rowIndex;
        currentPlant.OnDestructed += OnPlantDestructed;
        if (isBoosted)
        {
            BoostCurrentPlant();
        }
        return true;
    }

    public bool TryRemovePlant()
    {
        if (currentPlant == null) return false;

        currentPlant.ShovelDestruct();
        currentPlant = null;
        isBoosted = false;
        return true;
    }

    public bool TryBoost(BoostSun boostSun)
    {
        if (isBoosted || (currentPlant != null && currentPlant.HasBoosted)) return false;

        boostSun.AttachToCell(this);
        currentBoostSun = boostSun;
        isBoosted = true;
        if (currentPlant != null)
        {
            BoostCurrentPlant();
        }
        return true;
    }

    private void BoostCurrentPlant()
    {
        if (currentPlant == null) return;

        currentPlant.OnBoostCompleted += OnPlantBoostCompleted;
        currentPlant.Boost();
    }

    private void OnPlantBoostCompleted()
    {
        isBoosted = false;
    }

    private void OnPlantDestructed()
    {
        if (currentPlant != null)
        {
            currentPlant = null;
        }
        if (currentBoostSun != null)
        {
            currentBoostSun.Destruct();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSun : MonoBehaviour
{
    public const int BoostCost = 100;

    public void AttachToCell(Cell cell)
    {
        transform.position = cell.transform.position;
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}

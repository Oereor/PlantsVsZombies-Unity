using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostSun : MonoBehaviour
{
    public const int BoostCost = 100;

    public void AttachToCell(Cell cell)
    {
        transform.position = cell.transform.position;
        OnAttachedToCell?.Invoke();
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }

    public event UnityAction OnAttachedToCell;
}

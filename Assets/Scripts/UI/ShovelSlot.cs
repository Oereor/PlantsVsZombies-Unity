using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelSlot : MonoBehaviour
{
    public void OnClick()
    {
        HandManager.Instance.GrabOrDisposeShovel();
    }
}

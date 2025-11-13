using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostButton : MonoBehaviour
{
    public void OnClick()
    {
        if (SunManager.Instance.SunAmount < BoostSun.BoostCost) return;

        HandManager.Instance.GrabOrDisposeBoostSun();
    }
}

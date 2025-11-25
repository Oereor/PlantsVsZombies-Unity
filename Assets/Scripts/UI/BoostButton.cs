using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BoostButtonState
{
    CoolingDown,
    NeedSun,
    Active
}

public class BoostButton : MonoBehaviour
{
    private BoostButtonState currentState;

    [SerializeField] private Image buttonImage;
    [SerializeField] private float coolDownTime;
    private float coolDownTimer = 0;

    private void Start()
    {
        GameManager.Instance.OnStartPlanting += SwitchToCoolingDown;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BoostButtonState.CoolingDown:
                CoolingDownUpdate();
                break;
            case BoostButtonState.NeedSun:
                NeedSunUpdate();
                break;
            case BoostButtonState.Active:
                ActiveUpdate();
                break;
            default:
                break;
        }
    }

    private void CoolingDownUpdate()
    {
        coolDownTimer += Time.deltaTime;
        buttonImage.fillAmount = coolDownTimer / coolDownTime;
        if (coolDownTimer >= coolDownTime)
        {
            SwitchToActive();
            coolDownTimer = 0;
        }
    }

    private void NeedSunUpdate()
    {
        if (BoostSun.BoostCost <= SunManager.Instance.SunAmount)
        {
            SwitchToActive();
        }
    }

    private void ActiveUpdate()
    {
        if (BoostSun.BoostCost > SunManager.Instance.SunAmount)
        {
            SwitchToNeedSun();
        }
    }

    private void SwitchToActive()
    {
        currentState = BoostButtonState.Active;
        buttonImage.fillAmount = 1;
    }

    private void SwitchToCoolingDown()
    {
        currentState = BoostButtonState.CoolingDown;
        coolDownTimer = 0;
    }

    private void SwitchToNeedSun()
    {
        currentState = BoostButtonState.NeedSun;
        buttonImage.fillAmount = 0;
    }

    public void OnClick()
    {
        if (currentState != BoostButtonState.Active) return;

        BoostSun boostSun = HandManager.Instance.GrabOrDisposeBoostSun();
        if (boostSun != null)
        {
            boostSun.OnAttachedToCell += SwitchToCoolingDown;
        }
    }
}

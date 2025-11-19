using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CardState
{
    CoolingDown,
    NeedSun,
    Ready
}

public enum PlantType
{
    Jalapeno,
    CherryBomb,
    PotatoMine,
    IceShroom,
    Wallnut,
    Sunflower,
    PeaShooter
}

public class Card : MonoBehaviour
{
    private CardState currentState = CardState.CoolingDown;

    public PlantType plantType;

    public GameObject cardLight;

    public GameObject cardGrey;

    public Image cardDarkMask;

    [SerializeField] private float coolDownTime;

    private float cdTimer = 0;

    [SerializeField] private int sunCost;

    void Update()
    {
        switch (currentState)
        {
            case CardState.CoolingDown:
                CoolingDownUpdate();
                break;
            case CardState.NeedSun:
                NeedSunUpdate();
                break;
            case CardState.Ready:
                ReadyUpdate();
                break;
            default:
                break;
        }
    }

    void CoolingDownUpdate()
    {
        cdTimer += Time.deltaTime;

        cardDarkMask.fillAmount = 1 - (cdTimer / coolDownTime);
        if (cdTimer >= coolDownTime)
        {
            SwitchToNeedSun();
        }
    }

    void NeedSunUpdate()
    {
        if (sunCost <= SunManager.Instance.SunAmount)
        {
            SwitchToReady();
        }
    }

    void ReadyUpdate()
    {
        if (sunCost > SunManager.Instance.SunAmount)
        {
            SwitchToNeedSun();
        }
    }

    void SwitchToNeedSun()
    {
        currentState = CardState.NeedSun;

        cardLight.SetActive(false);
        cardGrey.SetActive(true);
        cardDarkMask.gameObject.SetActive(false);
    }

    void SwitchToReady()
    {
        currentState = CardState.Ready;

        cardLight.SetActive(true);
        cardGrey.SetActive(false);
        cardDarkMask.gameObject.SetActive(false);
    }

    void SwitchToCoolingDown()
    {
        currentState = CardState.CoolingDown;
        cdTimer = 0;

        cardLight.SetActive(false);
        cardGrey.SetActive(true);
        cardDarkMask.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if (sunCost > SunManager.Instance.SunAmount) return;

        Plant newPlant = HandManager.Instance.GrabOrDisposePlant(plantType);
        if (newPlant != null)
        {
            newPlant.OnPlanted += OnPlantSuccess;
        }
    }

    private void OnPlantSuccess()
    {
        SunManager.Instance.ConsumeSun(sunCost);
        SwitchToCoolingDown();
    }
}

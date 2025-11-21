using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    public static SunManager Instance { get; private set; }

    public GameObject sunPrefab;

    [SerializeField] private int sunAmount;
    public int SunAmount
    {
        get { return sunAmount; }
        private set
        {
            sunAmount = value;
            if (sunAmount < 0)
            {
                sunAmount = 0;
            }
            else if (sunAmount > 9990)
            {
                sunAmount = 9990;
            }
            UpdateSunAmountText();
        }
    }

    [SerializeField] private float sunAutoProduceInterval;
    private float sunAutoProduceTimer = 0;
    private float sunAutoProduceRandomOffset = 0;

    public TextMeshProUGUI sunAmountText;
    private Vector3 sunIconPosition;

    private bool canAutoProduce = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnStartPlanting += OnPlantingStart;
    }

    private void Update()
    {
        if (canAutoProduce)
        {
            AutoProduceSun();
        }
    }

    private void OnPlantingStart()
    {
        UpdateSunAmountText();
        CalculateSunIconPosition();
        EnableAutoProduce();
    }

    private void UpdateSunAmountText()
    {
        sunAmountText.text = sunAmount.ToString();
    }

    private void AutoProduceSun()
    {
        sunAutoProduceTimer += Time.deltaTime;
        if (sunAutoProduceTimer >= sunAutoProduceInterval + sunAutoProduceRandomOffset)
        {
            sunAutoProduceRandomOffset = Random.Range(0f, 3f);
            sunAutoProduceTimer = 0;
            Vector3 position = new Vector3(Random.Range(-4.5f, 6f), 6f, 0f);
            GameObject newSun = Instantiate(sunPrefab, position, Quaternion.identity);

            Vector3 targetPosition = new Vector3(position.x, Random.Range(-3.5f, 3f), 0f);
            newSun.GetComponent<Sun>().LinearMoveTo(targetPosition);
        }
    }

    public void ProduceSunAt(Vector3 position, int quantity)
    {
        if (quantity <= 0) return;

        for (int i = 0; i < quantity; i++)
        {
            ProduceSunAt(position);
        }
    }

    public void ProduceSunAt(Vector3 position)
    {
        GameObject newSun = Instantiate(sunPrefab, position, Quaternion.identity);

        float offset = Random.Range(0.3f, 1.3f);
        offset *= Random.Range(0, 2) == 0 ? -1 : 1;

        newSun.GetComponent<Sun>().JumpTo(new Vector3(position.x + offset, position.y, position.z));
    }

    public void EnableAutoProduce()
    {
        canAutoProduce = true;
    }

    public void ConsumeSun(int amount)
    {
        SunAmount -= amount;
    }

    public void CollectSun(int amount)
    {
        SunAmount += amount;
    }

    public Vector3 GetSunIconPosition()
    {
        return sunIconPosition;
    }

    private void CalculateSunIconPosition()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(sunAmountText.transform.position);
        position.z = 0;
        sunIconPosition = position;
    }
}

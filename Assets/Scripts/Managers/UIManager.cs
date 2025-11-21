using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CardListUI cardListUI;
    public GameStartCountdownUI gameStartCountdownUI;

    private void Awake()
    {
        Instance = this;
    }
}

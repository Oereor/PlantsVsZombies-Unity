using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        CameraManager.Instance.PeekZombies(onPeekCompleteCallback: ShowCountdown);
    }

    void ShowCountdown()
    {
        UIManager.Instance.cardListUI.ShowCardList();
        UIManager.Instance.gameStartCountdownUI.StartCountdown(onCountdownCompleteCallback: StartPlanting);
    }

    public void StartPlanting()
    {
        OnStartPlanting?.Invoke();
    }

    public event UnityAction OnStartPlanting;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStartCountdownUI : MonoBehaviour
{
    private Animator animator;

    private UnityAction onCountdownCompleteCallback;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void StartCountdown(UnityAction onCountdownCompleteCallback)
    {
        animator.enabled = true;
        this.onCountdownCompleteCallback = onCountdownCompleteCallback;
    }

    public void OnCountdownComplete()
    {
        onCountdownCompleteCallback?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private Camera mainCamera;

    private const float m_PeekCameraXPos = 4.71f;
    private const float m_NormalCameraXPos = -0.74f;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    public void PeekZombies(TweenCallback onPeekCompleteCallback)
    {
        Vector3 normalPosition = new Vector3(m_NormalCameraXPos, 0, -10), peekPosition = new Vector3(m_PeekCameraXPos, 0, -10);
        mainCamera.transform.DOPath(new Vector3[] { normalPosition, peekPosition }, 
            2f, PathType.CatmullRom).SetEase(Ease.InOutSine);
        mainCamera.transform.DOPath(new Vector3[] { peekPosition, normalPosition }, 
            2f, PathType.CatmullRom).SetEase(Ease.InOutSine).SetDelay(3f).OnComplete(onPeekCompleteCallback);
    }
}

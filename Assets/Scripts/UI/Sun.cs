using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sun : MonoBehaviour
{
    [SerializeField] private float jumpDuration;
    [SerializeField] private float collectDuration;
    [SerializeField] private float fallDuration;
    [SerializeField] private int sunValue;

    public void JumpTo(Vector3 targetPosition)
    {
        Vector3 centerPosition = (transform.position + targetPosition) / 2;
        float heightOffset = Vector3.Distance(transform.position, targetPosition) / 2;
        centerPosition.y += heightOffset;

        transform.DOPath(new Vector3[] { transform.position, centerPosition, targetPosition },
            jumpDuration, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(AutoCollect);
    }

    public void LinearMoveTo(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, fallDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(AutoCollect);
    }

    private void AutoCollect()
    {
        transform.DOMove(SunManager.Instance.GetSunIconPosition(), collectDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Destroy(gameObject);
                SunManager.Instance.CollectSun(sunValue);
            }
            );
    }
}

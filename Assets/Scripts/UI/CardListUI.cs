using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardListUI : MonoBehaviour
{
    private const int m_InScreenCardListYPos = 486;
    private const int m_OutScreenCardListYPos = 598;

    public Card[] cardList;

    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, m_OutScreenCardListYPos, transform.localPosition.z);
        DisableAllCards();
        GameManager.Instance.OnStartPlanting += EnableAllCards;
    }

    public void ShowCardList()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.DOLocalMoveY(m_InScreenCardListYPos, 0.5f).SetEase(Ease.OutBack);
        }
    }

    void DisableAllCards()
    {
        foreach (var card in cardList)
        {
            card.DisableCard();
        }
    }

    void EnableAllCards()
    {
        foreach (var card in cardList)
        {
            card.EnableCard();
        }
    }
}

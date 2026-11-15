using System.Collections.Generic;
using UnityEngine;

public class HandFanLayout : MonoBehaviour
{
    [SerializeField] private float arcRadius = 1200f;
    [SerializeField] private float anglePerCard = 5f;
    [SerializeField] private float maxTotalAngle = 30f;
    [SerializeField] private float hoverYOffset = 65f;

    private const int EmptyCount = 0;
    private const int SingleCardCount = 1;
    private const int IndexOffset = 1;
    private const int StartingIndex = 0;
    private const float ZeroValue = 0f;
    private const float HalfMultiplier = 0.5f;

    private readonly List<CardInteraction> cardsInHand = new List<CardInteraction>();

    public void AddCard(CardInteraction card)
    {
        if (!cardsInHand.Contains(card))
        {
            cardsInHand.Add(card);
            card.InitializeLayout(this);
            UpdateLayout();
        }
    }

    public void RemoveCard(CardInteraction card)
    {
        if (cardsInHand.Remove(card))
        {
            UpdateLayout();
        }
    }

    public void UpdateLayout()
    {
        List<CardInteraction> activeCards = new List<CardInteraction>();

        foreach (CardInteraction card in cardsInHand)
        {
            if (card != null && !card.IsDragging)
            {
                activeCards.Add(card);
            }
        }

        int cardCount = activeCards.Count;

        if (cardCount == EmptyCount)
        {
            return;
        }

        float totalAngle = cardCount > SingleCardCount
            ? Mathf.Min((cardCount - IndexOffset) * anglePerCard, maxTotalAngle)
            : ZeroValue;

        float startAngle = -totalAngle * HalfMultiplier;
        float angleStep = cardCount > SingleCardCount
            ? totalAngle / (cardCount - IndexOffset)
            : ZeroValue;

        CardInteraction hoveredCard = null;

        for (int i = StartingIndex; i < cardCount; i++)
        {
            CardInteraction currentCard = activeCards[i];
            float currentAngle = startAngle + (i * angleStep);
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            float positionX = Mathf.Sin(angleInRadians) * arcRadius;
            float positionY = (Mathf.Cos(angleInRadians) * arcRadius) - arcRadius;
            Quaternion targetRotation = Quaternion.Euler(ZeroValue, ZeroValue, -currentAngle);

            if (currentCard.IsHovered)
            {
                positionY += hoverYOffset;
                targetRotation = Quaternion.identity;
                hoveredCard = currentCard;
            }
            else
            {
                currentCard.transform.SetSiblingIndex(i);
            }

            Vector2 targetPosition = new Vector2(positionX, positionY);
            currentCard.SetTargetTransform(targetPosition, targetRotation, currentCard.IsHovered);
        }

        if (hoveredCard != null)
        {
            hoveredCard.transform.SetAsLastSibling();
        }
    }
}
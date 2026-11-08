using System.Collections.Generic;
using UnityEngine;

public class CardVisualManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private PlayZone playZone;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform handTransform;

    private Dictionary<CardInstance, Card> cardVisuals = new Dictionary<CardInstance, Card>();

    private void OnEnable()
    {
        cardManager.OnCardDrawn += SpawnCardInHand;
        cardManager.OnCardDiscarded += RemoveCardFromHand;
        cardManager.OnCardExhausted += RemoveCardFromHand;
        cardManager.OnCardStatsUpdated += UpdateCardsInHand;
        playZone.OnCardDropped += HandleCardDropped;

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged += UpdateCardsInHand;
        }
    }

    private void OnDisable()
    {
        cardManager.OnCardDrawn -= SpawnCardInHand;
        cardManager.OnCardDiscarded -= RemoveCardFromHand;
        cardManager.OnCardExhausted -= RemoveCardFromHand;
        cardManager.OnCardStatsUpdated -= UpdateCardsInHand;
        playZone.OnCardDropped -= HandleCardDropped;

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged -= UpdateCardsInHand;
        }
    }

    private void UpdateCardsInHand()
    {
        foreach (KeyValuePair<CardInstance, Card> pair in cardVisuals)
        {
            pair.Key.RecalculateStats(statusManager);
            pair.Value.RefreshStatsVisuals();
        }
    }

    private void HandleCardDropped(CardInstance cardInstance)
    {
        cardManager.PlayCard(cardInstance);
    }

    private void SpawnCardInHand(CardInstance cardInstance)
    {
        Card newCardVisual = Instantiate(cardPrefab, handTransform);

        if (statusManager != null)
        {
            cardInstance.RecalculateStats(statusManager);
        }

        newCardVisual.Initialize(cardInstance);
        cardVisuals.Add(cardInstance, newCardVisual);
    }

    private void RemoveCardFromHand(CardInstance cardInstance)
    {
        if (cardVisuals.ContainsKey(cardInstance))
        {
            Card visualToRemove = cardVisuals[cardInstance];
            Destroy(visualToRemove.gameObject);
            cardVisuals.Remove(cardInstance);
        }
    }
}
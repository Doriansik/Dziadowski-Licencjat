using System.Collections.Generic;
using UnityEngine;

public class CardVisualManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform handTransform;

    private Dictionary<CardInstance, Card> cardVisuals = new Dictionary<CardInstance, Card>();

    private void OnEnable()
    {
        cardManager.OnCardDrawn += SpawnCardInHand;
        cardManager.OnCardDiscarded += RemoveCardFromHand;
        cardManager.OnCardExhausted += RemoveCardFromHand;
    }

    private void OnDisable()
    {
        cardManager.OnCardDrawn -= SpawnCardInHand;
        cardManager.OnCardDiscarded -= RemoveCardFromHand;
        cardManager.OnCardExhausted -= RemoveCardFromHand;
    }

    private void SpawnCardInHand(CardInstance cardInstance)
    {
        Card newCardVisual = Instantiate(cardPrefab, handTransform);
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
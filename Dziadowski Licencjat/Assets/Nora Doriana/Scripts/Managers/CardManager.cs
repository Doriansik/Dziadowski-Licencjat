using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public event Action<CardInstance> OnCardPlayed;
    public event Action<CardInstance> OnCardDrawn;
    public event Action<CardInstance> OnCardDiscarded;
    public event Action<CardInstance> OnCardExhausted;
    public event Action OnDeckReshuffled;
    public event Action OnCardStatsUpdated;

    private List<CardInstance> drawPile = new List<CardInstance>();
    private List<CardInstance> hand = new List<CardInstance>();
    private List<CardInstance> discardPile = new List<CardInstance>();
    private List<CardInstance> exhaustPile = new List<CardInstance>();

    private IDeckShuffler deckShuffler;

    public IReadOnlyList<CardInstance> Hand => hand;

    public void Initialize(IEnumerable<CardSO> playerDeck, IDeckShuffler shuffler)
    {
        deckShuffler = shuffler;
        drawPile.Clear();
        hand.Clear();
        discardPile.Clear();
        exhaustPile.Clear();

        foreach (CardSO cardData in playerDeck)
        {
            CardInstance newCardInstance = new CardInstance(cardData);
            drawPile.Add(newCardInstance);
        }

        deckShuffler.Shuffle(drawPile);
    }

    public void StartBattle(int initialDrawAmount)
    {
        DrawCards(initialDrawAmount);
    }

    public void StartNewTurn(int turnDrawAmount)
    {
        DiscardEntireHand();
        DrawCards(turnDrawAmount);
    }

    public void DrawCards(int amount)
    {
        int minimumCardsToDraw = 0;
        int currentDrawnAmount = minimumCardsToDraw;

        while (currentDrawnAmount < amount)
        {
            int emptyPileThreshold = 0;

            if (drawPile.Count == emptyPileThreshold)
            {
                if (discardPile.Count == emptyPileThreshold)
                {
                    break;
                }

                ReshuffleDiscardIntoDrawPile();
            }

            int topCardIndex = drawPile.Count - 1;
            CardInstance drawnCard = drawPile[topCardIndex];

            drawPile.RemoveAt(topCardIndex);
            hand.Add(drawnCard);

            OnCardDrawn?.Invoke(drawnCard);
            currentDrawnAmount++;
        }
    }

    public void PlayCard(CardInstance card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            OnCardPlayed?.Invoke(card);

            if (card.BaseCard.isExhaust)
            {
                exhaustPile.Add(card);
                OnCardExhausted?.Invoke(card);
            }
            else
            {
                discardPile.Add(card);
                OnCardDiscarded?.Invoke(card);
            }
        }
    }

    public void DiscardCard(CardInstance card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);
            OnCardDiscarded?.Invoke(card);
        }
    }

    public void ExhaustCard(CardInstance card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            exhaustPile.Add(card);
            OnCardExhausted?.Invoke(card);
        }
    }

    public void DiscardEntireHand()
    {
        int emptyHandThreshold = 0;

        while (hand.Count > emptyHandThreshold)
        {
            int firstCardIndex = 0;
            CardInstance cardToDiscard = hand[firstCardIndex];
            DiscardCard(cardToDiscard);
        }
    }

    public void HandleScoopKeyword(int amount, CardSO saltCardData)
    {
        CardInstance saltCard = FindCardInAllPiles(saltCardData.cardName);

        if (saltCard != null)
        {
            saltCard.AddBonusDamage(amount);
            OnCardStatsUpdated?.Invoke();
        }
        else
        {
            CardInstance newSaltCard = new CardInstance(saltCardData);
            newSaltCard.AddBonusDamage(amount);
            hand.Add(newSaltCard);
            OnCardDrawn?.Invoke(newSaltCard);
        }
    }

    private CardInstance FindCardInAllPiles(string targetCardName)
    {
        foreach (CardInstance card in hand)
        {
            if (card.BaseCard.cardName == targetCardName) return card;
        }

        foreach (CardInstance card in drawPile)
        {
            if (card.BaseCard.cardName == targetCardName) return card;
        }

        foreach (CardInstance card in discardPile)
        {
            if (card.BaseCard.cardName == targetCardName) return card;
        }

        foreach (CardInstance card in exhaustPile)
        {
            if (card.BaseCard.cardName == targetCardName) return card;
        }

        return null;
    }

    private void ReshuffleDiscardIntoDrawPile()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        deckShuffler.Shuffle(drawPile);
        OnDeckReshuffled?.Invoke();
    }
}
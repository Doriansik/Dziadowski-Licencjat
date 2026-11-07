using System.Collections.Generic;
using UnityEngine;

public class CardManagerTester : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private List<CardSO> startingDeck;

    private void Start()
    {
        StandardDeckShuffler shuffler = new StandardDeckShuffler();

        cardManager.OnCardDrawn += HandleCardDrawn;
        cardManager.OnCardDiscarded += HandleCardDiscarded;
        cardManager.OnCardExhausted += HandleCardExhausted;
        cardManager.OnDeckReshuffled += HandleDeckReshuffled;

        cardManager.Initialize(startingDeck, shuffler);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            int cardsToDraw = 1;
            cardManager.DrawCards(cardsToDraw);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            cardManager.DiscardEntireHand();
        }
    }

    private void HandleCardDrawn(CardInstance card)
    {
        Debug.Log("Drawn card: " + card.BaseCard.cardName);
    }

    private void HandleCardDiscarded(CardInstance card)
    {
        Debug.Log("Discarded card: " + card.BaseCard.cardName);
    }

    private void HandleCardExhausted(CardInstance card)
    {
        Debug.Log("Exhausted card: " + card.BaseCard.cardName);
    }

    private void HandleDeckReshuffled()
    {
        Debug.Log("Deck Reshuffled");
    }

    private void OnDestroy()
    {
        if (cardManager != null)
        {
            cardManager.OnCardDrawn -= HandleCardDrawn;
            cardManager.OnCardDiscarded -= HandleCardDiscarded;
            cardManager.OnCardExhausted -= HandleCardExhausted;
            cardManager.OnDeckReshuffled -= HandleDeckReshuffled;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class CardManagerTester : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private List<CardSO> startingDeck;
    [SerializeField] private int startingHandSize = 5;
    [SerializeField] private int cardsDrawnPerTurn = 5;
    [SerializeField] private int testVulnerabilityAmount = 50;
    [SerializeField] private int testWeaknessAmount = 25;
    [SerializeField] private int startingMana = 3;

    private void Start()
    {
        StandardDeckShuffler shuffler = new StandardDeckShuffler();

        cardManager.OnCardDrawn += HandleCardDrawn;
        cardManager.OnCardDiscarded += HandleCardDiscarded;
        cardManager.OnCardExhausted += HandleCardExhausted;
        cardManager.OnDeckReshuffled += HandleDeckReshuffled;

        manaManager.Initialize(startingMana);
        cardManager.Initialize(startingDeck, shuffler);
        cardManager.StartBattle(startingHandSize);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            manaManager.RestoreFullMana();
            cardManager.StartNewTurn(cardsDrawnPerTurn);
            Debug.Log("Started new turn! Mana restored.");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            statusManager.AddEnemyVulnerability(testVulnerabilityAmount);
            Debug.Log("Added Enemy Vulnerability!");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            statusManager.AddPlayerWeakness(testWeaknessAmount);
            Debug.Log("Added Player Weakness!");
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
        Debug.Log("Deck reshuffled!");
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
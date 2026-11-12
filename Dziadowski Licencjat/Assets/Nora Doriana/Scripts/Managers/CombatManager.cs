using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public event Action OnCombatVictory;
    public event Action OnCombatDefeat;

    [SerializeField] private CardManager cardManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private StatusManager statusManager;

    [SerializeField] private List<DeckEntry> startingDeck;
    [SerializeField] private EnemySO startingEnemy;

    [SerializeField] private int startingHandSize = 5;
    [SerializeField] private int cardsDrawnPerTurn = 5;
    [SerializeField] private int startingMana = 3;
    [SerializeField] private int startingPlayerHealth = 100;

    public CombatState CurrentState { get; private set; }

    private void Start()
    {
        CurrentState = CombatState.Initialize;

        playerManager.OnDied += HandlePlayerDied;

        StandardDeckShuffler shuffler = new StandardDeckShuffler();

        List<CardSO> unpackedDeck = UnpackStartingDeck();
        cardManager.Initialize(unpackedDeck, shuffler);

        playerManager.Initialize(startingPlayerHealth);
        manaManager.Initialize(startingMana);

        enemyManager.SpawnEnemy(startingEnemy);
        enemyManager.CurrentEnemy.OnDied += HandleEnemyDied;

        cardManager.StartBattle(startingHandSize);

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        if (CurrentState == CombatState.Victory || CurrentState == CombatState.Defeat)
        {
            return;
        }

        CurrentState = CombatState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        if (CurrentState != CombatState.PlayerTurn)
        {
            return;
        }

        CurrentState = CombatState.EnemyTurn;

        statusManager.DecreasePlayerStatuses();
        enemyManager.ExecuteEnemyTurn();

        StartTurnAction startTurnAction = new StartTurnAction(playerManager, manaManager, cardManager, this, statusManager, cardsDrawnPerTurn);
        actionQueueManager.AddAction(startTurnAction);
    }

    private List<CardSO> UnpackStartingDeck()
    {
        List<CardSO> unpackedDeck = new List<CardSO>();
        int startingIndex = 0;

        foreach (DeckEntry entry in startingDeck)
        {
            if (entry.cardData != null)
            {
                for (int i = startingIndex; i < entry.amount; i++)
                {
                    unpackedDeck.Add(entry.cardData);
                }
            }
        }

        return unpackedDeck;
    }

    private void HandlePlayerDied()
    {
        CurrentState = CombatState.Defeat;
        OnCombatDefeat?.Invoke();
    }

    private void HandleEnemyDied()
    {
        CurrentState = CombatState.Victory;
        OnCombatVictory?.Invoke();
    }

    private void OnDestroy()
    {
        if (playerManager != null)
        {
            playerManager.OnDied -= HandlePlayerDied;
        }

        if (enemyManager != null && enemyManager.CurrentEnemy != null)
        {
            enemyManager.CurrentEnemy.OnDied -= HandleEnemyDied;
        }
    }
}
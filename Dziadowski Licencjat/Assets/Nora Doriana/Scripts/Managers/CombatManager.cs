using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private StatusManager statusManager;

    [SerializeField] private List<CardSO> startingDeck;
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
        cardManager.Initialize(startingDeck, shuffler);

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
        Debug.Log("Player turn started!");
    }

    public void EndPlayerTurn()
    {
        if (CurrentState != CombatState.PlayerTurn)
        {
            return;
        }

        CurrentState = CombatState.EnemyTurn;
        Debug.Log("Player turn ended. Enemy turn starting.");

        enemyManager.ExecuteEnemyTurn();

        StartTurnAction startTurnAction = new StartTurnAction(playerManager, manaManager, cardManager, this, cardsDrawnPerTurn);
        actionQueueManager.AddAction(startTurnAction);
    }

    private void HandlePlayerDied()
    {
        CurrentState = CombatState.Defeat;
        Debug.Log("Defeat! Player died.");
    }

    private void HandleEnemyDied()
    {
        CurrentState = CombatState.Victory;
        Debug.Log("Victory! Enemy died.");
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
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action<EnemyInstance> OnEnemySpawned;

    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private StatusManager statusManager;

    public EnemyInstance CurrentEnemy { get; private set; }

    private IEnemyMoveSelector moveSelector;

    public void SpawnEnemy(EnemySO enemyData)
    {
        CurrentEnemy = new EnemyInstance(enemyData);
        CurrentEnemy.OnDied += HandleEnemyDied;

        moveSelector = CreateMoveSelector(enemyData.AIMode);

        OnEnemySpawned?.Invoke(CurrentEnemy);
        GenerateNextIntent();
    }

    public void GenerateNextIntent()
    {
        if (CurrentEnemy == null || moveSelector == null)
        {
            return;
        }

        EnemyMove nextMove = moveSelector.SelectNextMove(CurrentEnemy);
        if (nextMove != null)
        {
            CurrentEnemy.SetNextMove(nextMove);
        }
    }

    public void ExecuteEnemyTurn()
    {
        CurrentEnemy.ResetBlock();

        EnemyTurnAction turnAction = new EnemyTurnAction(CurrentEnemy, this, playerManager, statusManager);
        actionQueueManager.AddAction(turnAction);
    }

    private IEnemyMoveSelector CreateMoveSelector(EnemyAIMode aiMode)
    {
        if (aiMode == EnemyAIMode.ScriptedSequence)
        {
            return new ScriptedEnemyMoveSelector();
        }

        return new RandomEnemyMoveSelector();
    }

    private void HandleEnemyDied()
    {
        CurrentEnemy.OnDied -= HandleEnemyDied;
    }

    private void OnDestroy()
    {
        if (CurrentEnemy != null)
        {
            CurrentEnemy.OnDied -= HandleEnemyDied;
        }
    }
}
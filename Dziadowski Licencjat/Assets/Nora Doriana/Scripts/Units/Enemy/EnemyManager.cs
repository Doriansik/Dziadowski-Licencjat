using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action<EnemyInstance> OnEnemySpawned;

    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private StatusManager statusManager;

    public EnemyInstance CurrentEnemy { get; private set; }

    public void SpawnEnemy(EnemySO enemyData)
    {
        CurrentEnemy = new EnemyInstance(enemyData);
        CurrentEnemy.OnDied += HandleEnemyDied;

        OnEnemySpawned?.Invoke(CurrentEnemy);
        GenerateNextIntent();
    }

    public void GenerateNextIntent()
    {
        int minimumIndex = 0;
        int maximumIndex = CurrentEnemy.BaseEnemy.possibleMoves.Count;

        if (maximumIndex > minimumIndex)
        {
            int randomIndex = UnityEngine.Random.Range(minimumIndex, maximumIndex);
            EnemyMove nextMove = CurrentEnemy.BaseEnemy.possibleMoves[randomIndex];
            CurrentEnemy.SetNextMove(nextMove);
        }
    }

    public void ExecuteEnemyTurn()
    {
        CurrentEnemy.ResetBlock();

        EnemyTurnAction turnAction = new EnemyTurnAction(CurrentEnemy, this, playerManager, statusManager);
        actionQueueManager.AddAction(turnAction);
    }

    private void HandleEnemyDied()
    {
        Debug.Log("Enemy defeated!");
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
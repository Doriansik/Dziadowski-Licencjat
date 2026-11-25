using UnityEngine;

public class EnemyTurnAction : IGameAction
{
    private const int StartingHitIndex = 0;

    public bool IsCompleted { get; private set; }

    private EnemyInstance enemy;
    private EnemyManager enemyManager;
    private PlayerManager playerManager;
    private StatusManager statusManager;

    public EnemyTurnAction(EnemyInstance enemy, EnemyManager enemyManager, PlayerManager playerManager, StatusManager statusManager)
    {
        this.enemy = enemy;
        this.enemyManager = enemyManager;
        this.playerManager = playerManager;
        this.statusManager = statusManager;
        IsCompleted = false;
    }

    public void Execute()
    {
        EnemyMove currentMove = enemy.CurrentMove;

        if (currentMove.intentType == EnemyIntentType.Attack || currentMove.intentType == EnemyIntentType.SpecialAttack)
        {
            ExecuteAttack(currentMove);
        }
        else if (currentMove.intentType == EnemyIntentType.Defend)
        {
            enemy.AddBlock(currentMove.value);
        }
        else if (currentMove.intentType == EnemyIntentType.Heal)
        {
            enemy.Heal(currentMove.value);
        }
        else if (currentMove.intentType == EnemyIntentType.Debuff)
        {
            if (currentMove.statusType == StatusType.Weakness)
            {
                statusManager.AddPlayerWeakness(currentMove.value);
            }
            else if (currentMove.statusType == StatusType.Vulnerability)
            {
                statusManager.AddPlayerVulnerability(currentMove.value);
            }
        }

        enemyManager.GenerateNextIntent();
        IsCompleted = true;
    }

    private void ExecuteAttack(EnemyMove move)
    {
        int finalDamage = move.CalculateEffectiveDamage(statusManager);
        int hits = move.GetEffectiveHitCount();

        for (int i = StartingHitIndex; i < hits; i++)
        {
            playerManager.TakeDamage(finalDamage);
        }
    }

    public void UpdateAction()
    {
    }
}
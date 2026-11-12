using UnityEngine;

public class EnemyTurnAction : IGameAction
{
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

        if (currentMove.intentType == EnemyIntentType.Attack)
        {
            float weaknessMultiplier = statusManager.GetEnemyDamageMultiplier();
            float vulnerabilityMultiplier = statusManager.GetPlayerVulnerabilityMultiplier();

            float finalDamageFloat = currentMove.value * weaknessMultiplier * vulnerabilityMultiplier;
            int finalDamage = Mathf.RoundToInt(finalDamageFloat);

            playerManager.TakeDamage(finalDamage);
        }
        else if (currentMove.intentType == EnemyIntentType.Defend)
        {
            enemy.AddBlock(currentMove.value);
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

    public void UpdateAction()
    {
    }
}
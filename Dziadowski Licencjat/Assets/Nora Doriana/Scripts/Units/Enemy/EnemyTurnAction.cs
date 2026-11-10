using UnityEngine;

public class EnemyTurnAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private EnemyInstance enemy;
    private EnemyManager enemyManager;
    private PlayerManager playerManager;
    private StatusManager statusManager;

    private const float BaseMultiplier = 1f;
    private const float PercentageDivisor = 100f;

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
            float weaknessMultiplier = BaseMultiplier - (statusManager.EnemyWeaknessPercentage / PercentageDivisor);
            float vulnerabilityMultiplier = BaseMultiplier + (statusManager.PlayerVulnerabilityPercentage / PercentageDivisor);

            float finalDamageFloat = currentMove.value * weaknessMultiplier * vulnerabilityMultiplier;
            int finalDamage = Mathf.RoundToInt(finalDamageFloat);

            playerManager.TakeDamage(finalDamage);
            Debug.Log("Enemy attacks player for " + finalDamage + " damage! Player HP: " + playerManager.CurrentHealth);
        }
        else if (currentMove.intentType == EnemyIntentType.Defend)
        {
            enemy.AddBlock(currentMove.value);
            Debug.Log("Enemy gains " + currentMove.value + " block!");
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

            Debug.Log("Enemy debuffs player with " + currentMove.value + " " + currentMove.statusType);
        }
        else if (currentMove.intentType == EnemyIntentType.Buff)
        {
            Debug.Log("Enemy buffs itself!");
        }

        enemyManager.GenerateNextIntent();
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
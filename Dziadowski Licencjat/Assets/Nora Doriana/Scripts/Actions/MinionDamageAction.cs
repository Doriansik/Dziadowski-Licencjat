using UnityEngine;

public class MinionDamageAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private MinionManager minionManager;
    private EnemyManager enemyManager;
    private int damageAmount;

    public MinionDamageAction(MinionManager minionManager, EnemyManager enemyManager, int amount)
    {
        this.minionManager = minionManager;
        this.enemyManager = enemyManager;
        damageAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        if (minionManager.IsAlive && enemyManager.CurrentEnemy != null)
        {
            enemyManager.CurrentEnemy.TakeDamage(damageAmount);
        }

        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
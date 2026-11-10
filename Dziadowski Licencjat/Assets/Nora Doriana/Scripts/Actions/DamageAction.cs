using UnityEngine;

public class DamageAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private int damageAmount;
    private EnemyManager targetEnemy;

    public DamageAction(int damage, EnemyManager enemy)
    {
        damageAmount = damage;
        targetEnemy = enemy;
        IsCompleted = false;
    }

    public void Execute()
    {
        if (targetEnemy != null && targetEnemy.CurrentEnemy != null)
        {
            targetEnemy.CurrentEnemy.TakeDamage(damageAmount);
            Debug.Log("Dealt " + damageAmount + " damage to the enemy. Enemy HP: " + targetEnemy.CurrentEnemy.CurrentHealth);
        }

        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
using UnityEngine;

public class DamageAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private float damageAmount;

    public DamageAction(float damage)
    {
        damageAmount = damage;
        IsCompleted = false;
    }

    public void Execute()
    {
        Debug.Log("Dealing " + damageAmount + " damage to the enemy!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
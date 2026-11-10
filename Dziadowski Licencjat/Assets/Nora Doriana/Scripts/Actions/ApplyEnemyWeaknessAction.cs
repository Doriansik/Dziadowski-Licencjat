using UnityEngine;

public class ApplyEnemyWeaknessAction : IGameAction
{
    private StatusManager statusManager;
    private int weaknessAmount;
    public bool IsCompleted { get; private set; }

    public ApplyEnemyWeaknessAction(StatusManager statusManager, int amount)
    {
        this.statusManager = statusManager;
        weaknessAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        statusManager.AddEnemyWeakness(weaknessAmount);
        Debug.Log("Applied " + weaknessAmount + " weakness to enemy!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
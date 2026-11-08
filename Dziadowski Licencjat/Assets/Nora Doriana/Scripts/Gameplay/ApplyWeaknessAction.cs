using UnityEngine;

public class ApplyWeaknessAction : IGameAction
{
    private StatusManager statusManager;
    private int weaknessAmount;
    public bool IsCompleted { get; private set; }

    public ApplyWeaknessAction(StatusManager statusManager, int amount)
    {
        this.statusManager = statusManager;
        weaknessAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        statusManager.AddPlayerWeakness(weaknessAmount);
        Debug.Log("Applied " + weaknessAmount + " weakness to player!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
using UnityEngine;

public class SummonAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private MinionManager minionManager;
    private int summonHealth;

    public SummonAction(MinionManager minionManager, int amount)
    {
        this.minionManager = minionManager;
        summonHealth = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        minionManager.Spawn(summonHealth);
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
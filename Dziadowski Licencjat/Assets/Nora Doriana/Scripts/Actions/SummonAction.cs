using UnityEngine;

public class SummonAction : IGameAction
{
    private int summonAmount;
    public bool IsCompleted { get; private set; }

    public SummonAction(int amount)
    {
        summonAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        Debug.Log("Summoned Duszyczka with " + summonAmount + " HP / Increased max HP!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
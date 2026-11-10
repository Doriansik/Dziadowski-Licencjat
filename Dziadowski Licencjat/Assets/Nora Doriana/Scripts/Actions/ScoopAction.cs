using UnityEngine;

public class ScoopAction : IGameAction
{
    private CardManager cardManager;
    private CardSO saltCardData;
    private int scoopAmount;
    public bool IsCompleted { get; private set; }

    public ScoopAction(CardManager cardManager, CardSO saltCardData, int amount)
    {
        this.cardManager = cardManager;
        this.saltCardData = saltCardData;
        scoopAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        cardManager.HandleScoopKeyword(scoopAmount, saltCardData);
        Debug.Log("Scooped " + scoopAmount + " salt damage!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
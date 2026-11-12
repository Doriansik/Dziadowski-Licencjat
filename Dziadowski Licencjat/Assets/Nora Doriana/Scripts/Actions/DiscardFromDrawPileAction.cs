using UnityEngine;

public class DiscardFromDrawPileAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private CardManager cardManager;
    private int amountToDiscard;

    public DiscardFromDrawPileAction(CardManager cardManager, int amount)
    {
        this.cardManager = cardManager;
        amountToDiscard = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        cardManager.DiscardFromDrawPile(amountToDiscard);
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
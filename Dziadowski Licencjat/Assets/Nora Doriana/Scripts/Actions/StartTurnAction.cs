using UnityEngine;

public class StartTurnAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private PlayerManager playerManager;
    private ManaManager manaManager;
    private CardManager cardManager;
    private CombatManager combatManager;
    private int drawAmount;

    public StartTurnAction(PlayerManager playerManager, ManaManager manaManager, CardManager cardManager, CombatManager combatManager, int drawAmount)
    {
        this.playerManager = playerManager;
        this.manaManager = manaManager;
        this.cardManager = cardManager;
        this.combatManager = combatManager;
        this.drawAmount = drawAmount;
        IsCompleted = false;
    }

    public void Execute()
    {
        manaManager.RestoreFullMana();
        playerManager.ResetBlock();
        cardManager.StartNewTurn(drawAmount);

        combatManager.StartPlayerTurn();

        Debug.Log("Turn reset actions completed.");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
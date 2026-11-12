using UnityEngine;

public class StartTurnAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private PlayerManager playerManager;
    private ManaManager manaManager;
    private CardManager cardManager;
    private CombatManager combatManager;
    private StatusManager statusManager;
    private int drawAmount;

    public StartTurnAction(PlayerManager playerManager, ManaManager manaManager, CardManager cardManager, CombatManager combatManager, StatusManager statusManager, int drawAmount)
    {
        this.playerManager = playerManager;
        this.manaManager = manaManager;
        this.cardManager = cardManager;
        this.combatManager = combatManager;
        this.statusManager = statusManager;
        this.drawAmount = drawAmount;
        IsCompleted = false;
    }

    public void Execute()
    {
        statusManager.DecreaseEnemyStatuses();

        manaManager.RestoreFullMana();
        playerManager.ResetBlock();
        cardManager.StartNewTurn(drawAmount);

        combatManager.StartPlayerTurn();

        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
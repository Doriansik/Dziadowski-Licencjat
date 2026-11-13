using UnityEngine;

public class StartTurnAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private PlayerManager playerManager;
    private MinionManager minionManager;
    private ManaManager manaManager;
    private CardManager cardManager;
    private CombatManager combatManager;
    private StatusManager statusManager;
    private int drawAmount;

    public StartTurnAction(PlayerManager playerManager, MinionManager minionManager, ManaManager manaManager, CardManager cardManager, CombatManager combatManager, StatusManager statusManager, int drawAmount)
    {
        this.playerManager = playerManager;
        this.minionManager = minionManager;
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

        if (minionManager != null)
        {
            minionManager.ResetBlock();
        }

        cardManager.StartNewTurn(drawAmount);

        combatManager.StartPlayerTurn();

        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
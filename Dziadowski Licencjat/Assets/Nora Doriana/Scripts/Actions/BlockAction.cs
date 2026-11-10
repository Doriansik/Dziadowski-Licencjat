using UnityEngine;

public class BlockAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private int blockAmount;
    private PlayerManager playerManager;

    public BlockAction(int block, PlayerManager playerManager)
    {
        blockAmount = block;
        this.playerManager = playerManager;
        IsCompleted = false;
    }

    public void Execute()
    {
        playerManager.AddBlock(blockAmount);
        Debug.Log("Player gained " + blockAmount + " block!");
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
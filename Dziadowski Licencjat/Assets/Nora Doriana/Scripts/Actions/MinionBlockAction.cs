public class MinionBlockAction : IGameAction
{
    public bool IsCompleted { get; private set; }

    private MinionManager minionManager;
    private int blockAmount;

    public MinionBlockAction(MinionManager minionManager, int amount)
    {
        this.minionManager = minionManager;
        blockAmount = amount;
        IsCompleted = false;
    }

    public void Execute()
    {
        minionManager.AddBlock(blockAmount);
        IsCompleted = true;
    }

    public void UpdateAction()
    {
    }
}
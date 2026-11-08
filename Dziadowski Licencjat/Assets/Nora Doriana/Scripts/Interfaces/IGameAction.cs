public interface IGameAction
{
    bool IsCompleted { get; }
    void Execute();
    void UpdateAction();
}
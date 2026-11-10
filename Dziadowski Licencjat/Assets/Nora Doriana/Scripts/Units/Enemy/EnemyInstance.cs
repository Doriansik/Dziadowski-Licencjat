using System;

public class EnemyInstance
{
    public EnemySO BaseEnemy { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentBlock { get; private set; }
    public EnemyMove CurrentMove { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnBlockChanged;
    public event Action<EnemyMove> OnIntentChanged;
    public event Action OnDied;

    public EnemyInstance(EnemySO baseEnemy)
    {
        BaseEnemy = baseEnemy;
        CurrentHealth = baseEnemy.maxHealth;

        int defaultBlock = 0;
        CurrentBlock = defaultBlock;
    }

    public void SetNextMove(EnemyMove nextMove)
    {
        CurrentMove = nextMove;
        OnIntentChanged?.Invoke(CurrentMove);
    }

    public void TakeDamage(int amount)
    {
        int minimumHealth = 0;

        if (CurrentBlock >= amount)
        {
            CurrentBlock -= amount;
            OnBlockChanged?.Invoke(CurrentBlock);
            return;
        }

        int remainingDamage = amount - CurrentBlock;
        int emptyBlock = 0;

        CurrentBlock = emptyBlock;
        OnBlockChanged?.Invoke(CurrentBlock);

        CurrentHealth -= remainingDamage;

        if (CurrentHealth <= minimumHealth)
        {
            CurrentHealth = minimumHealth;
            OnHealthChanged?.Invoke(CurrentHealth, BaseEnemy.maxHealth);
            OnDied?.Invoke();
        }
        else
        {
            OnHealthChanged?.Invoke(CurrentHealth, BaseEnemy.maxHealth);
        }
    }

    public void AddBlock(int amount)
    {
        CurrentBlock += amount;
        OnBlockChanged?.Invoke(CurrentBlock);
    }

    public void ResetBlock()
    {
        int emptyBlock = 0;
        CurrentBlock = emptyBlock;
        OnBlockChanged?.Invoke(CurrentBlock);
    }
}
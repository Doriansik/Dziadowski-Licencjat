using System;
using UnityEngine;

public class EnemyInstance
{
    private const int InitialSequenceIndex = 0;
    private const int InitialRepeatCount = 1;
    private const int SequenceStep = 1;

    public EnemySO BaseEnemy { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentBlock { get; private set; }
    public EnemyMove CurrentMove { get; private set; }
    public EnemyMove PreviousMove { get; private set; }
    public int MoveSequenceIndex { get; private set; }
    public int ConsecutiveSameMoveCount { get; private set; }

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
        MoveSequenceIndex = InitialSequenceIndex;
    }

    public void SetNextMove(EnemyMove nextMove)
    {
        if (nextMove == PreviousMove)
        {
            ConsecutiveSameMoveCount++;
        }
        else
        {
            ConsecutiveSameMoveCount = InitialRepeatCount;
        }

        PreviousMove = nextMove;
        CurrentMove = nextMove;
        OnIntentChanged?.Invoke(CurrentMove);
    }

    public void AdvanceSequenceIndex(int totalMovesCount)
    {
        int minimumCount = 0;
        if (totalMovesCount <= minimumCount)
        {
            return;
        }

        MoveSequenceIndex = (MoveSequenceIndex + SequenceStep) % totalMovesCount;
    }

    public void Heal(int amount)
    {
        int minimumHealth = 0;
        if (CurrentHealth <= minimumHealth)
        {
            return;
        }

        CurrentHealth = Mathf.Min(CurrentHealth + amount, BaseEnemy.maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, BaseEnemy.maxHealth);
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
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnBlockChanged;
    public event Action OnDied;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentBlock { get; private set; }

    public void Initialize(int startingMaxHealth)
    {
        MaxHealth = startingMaxHealth;
        CurrentHealth = MaxHealth;

        int defaultBlock = 0;
        CurrentBlock = defaultBlock;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnBlockChanged?.Invoke(CurrentBlock);
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
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            OnDied?.Invoke();
        }
        else
        {
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
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
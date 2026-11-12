using System;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnBlockChanged;
    public event Action OnSpawned;
    public event Action OnDied;

    public bool IsAlive { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int CurrentBlock { get; private set; }

    public void Spawn(int health)
    {
        MaxHealth = health;
        CurrentHealth = health;
        IsAlive = true;

        int defaultBlock = 0;
        CurrentBlock = defaultBlock;

        OnSpawned?.Invoke();
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        OnBlockChanged?.Invoke(CurrentBlock);
    }

    public void AddBlock(int amount)
    {
        if (!IsAlive)
        {
            return;
        }

        CurrentBlock += amount;
        OnBlockChanged?.Invoke(CurrentBlock);
    }

    public void ResetBlock()
    {
        int emptyBlock = 0;
        CurrentBlock = emptyBlock;

        if (IsAlive)
        {
            OnBlockChanged?.Invoke(CurrentBlock);
        }
    }

    public int TakeDamage(int amount)
    {
        if (!IsAlive)
        {
            return amount;
        }

        int minimumHealth = 0;
        int remainingDamage = amount;

        if (CurrentBlock >= remainingDamage)
        {
            CurrentBlock -= remainingDamage;
            OnBlockChanged?.Invoke(CurrentBlock);
            return minimumHealth;
        }

        remainingDamage -= CurrentBlock;
        int emptyBlock = 0;

        CurrentBlock = emptyBlock;
        OnBlockChanged?.Invoke(CurrentBlock);

        if (CurrentHealth >= remainingDamage)
        {
            CurrentHealth -= remainingDamage;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth == minimumHealth)
            {
                Die();
            }

            return minimumHealth;
        }

        remainingDamage -= CurrentHealth;
        CurrentHealth = minimumHealth;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        Die();

        return remainingDamage;
    }

    private void Die()
    {
        IsAlive = false;
        OnDied?.Invoke();
    }
}
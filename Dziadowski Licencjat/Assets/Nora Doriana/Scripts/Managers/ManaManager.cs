using System;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    public event Action<int, int> OnManaChanged;

    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }

    public void Initialize(int startingMaxMana)
    {
        MaxMana = startingMaxMana;
        RestoreFullMana();
    }

    public void RestoreFullMana()
    {
        CurrentMana = MaxMana;
        OnManaChanged?.Invoke(CurrentMana, MaxMana);
    }

    public bool TrySpendMana(int amount)
    {
        if (CurrentMana >= amount)
        {
            CurrentMana -= amount;
            OnManaChanged?.Invoke(CurrentMana, MaxMana);
            return true;
        }

        Debug.Log("Not enough mana!");
        return false;
    }
}
using System;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public event Action OnStatusesChanged;

    public int EnemyVulnerabilityPercentage { get; private set; }
    public int PlayerWeaknessPercentage { get; private set; }

    public void AddEnemyVulnerability(int percentage)
    {
        EnemyVulnerabilityPercentage += percentage;
        OnStatusesChanged?.Invoke();
    }

    public void AddPlayerWeakness(int percentage)
    {
        PlayerWeaknessPercentage += percentage;
        OnStatusesChanged?.Invoke();
    }

    public void ResetStatuses()
    {
        int defaultPercentage = 0;
        EnemyVulnerabilityPercentage = defaultPercentage;
        PlayerWeaknessPercentage = defaultPercentage;
        OnStatusesChanged?.Invoke();
    }
}
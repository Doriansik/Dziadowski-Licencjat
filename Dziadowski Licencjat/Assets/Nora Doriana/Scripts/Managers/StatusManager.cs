using System;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public event Action OnStatusesChanged;

    public int EnemyVulnerabilityTurns { get; private set; }
    public int EnemyWeaknessTurns { get; private set; }
    public int PlayerWeaknessTurns { get; private set; }
    public int PlayerVulnerabilityTurns { get; private set; }

    private const float VulnerabilityMultiplier = 1.5f;
    private const float WeaknessMultiplier = 0.75f;
    private const float DefaultMultiplier = 1f;

    public void AddEnemyVulnerability(int turns)
    {
        EnemyVulnerabilityTurns += turns;
        OnStatusesChanged?.Invoke();
    }

    public void AddEnemyWeakness(int turns)
    {
        EnemyWeaknessTurns += turns;
        OnStatusesChanged?.Invoke();
    }

    public void AddPlayerWeakness(int turns)
    {
        PlayerWeaknessTurns += turns;
        OnStatusesChanged?.Invoke();
    }

    public void AddPlayerVulnerability(int turns)
    {
        PlayerVulnerabilityTurns += turns;
        OnStatusesChanged?.Invoke();
    }

    public void DecreasePlayerStatuses()
    {
        int minimumTurns = 0;

        if (PlayerWeaknessTurns > minimumTurns) PlayerWeaknessTurns--;
        if (PlayerVulnerabilityTurns > minimumTurns) PlayerVulnerabilityTurns--;

        OnStatusesChanged?.Invoke();
    }

    public void DecreaseEnemyStatuses()
    {
        int minimumTurns = 0;

        if (EnemyWeaknessTurns > minimumTurns) EnemyWeaknessTurns--;
        if (EnemyVulnerabilityTurns > minimumTurns) EnemyVulnerabilityTurns--;

        OnStatusesChanged?.Invoke();
    }

    public void ResetStatuses()
    {
        int defaultTurns = 0;
        EnemyVulnerabilityTurns = defaultTurns;
        EnemyWeaknessTurns = defaultTurns;
        PlayerWeaknessTurns = defaultTurns;
        PlayerVulnerabilityTurns = defaultTurns;
        OnStatusesChanged?.Invoke();
    }

    public float GetPlayerDamageMultiplier()
    {
        float multiplier = DefaultMultiplier;
        int minimumTurns = 0;

        if (PlayerWeaknessTurns > minimumTurns)
        {
            multiplier *= WeaknessMultiplier;
        }

        return multiplier;
    }

    public float GetEnemyDamageMultiplier()
    {
        float multiplier = DefaultMultiplier;
        int minimumTurns = 0;

        if (EnemyWeaknessTurns > minimumTurns)
        {
            multiplier *= WeaknessMultiplier;
        }

        return multiplier;
    }

    public float GetPlayerVulnerabilityMultiplier()
    {
        float multiplier = DefaultMultiplier;
        int minimumTurns = 0;

        if (PlayerVulnerabilityTurns > minimumTurns)
        {
            multiplier *= VulnerabilityMultiplier;
        }

        return multiplier;
    }

    public float GetEnemyVulnerabilityMultiplier()
    {
        float multiplier = DefaultMultiplier;
        int minimumTurns = 0;

        if (EnemyVulnerabilityTurns > minimumTurns)
        {
            multiplier *= VulnerabilityMultiplier;
        }

        return multiplier;
    }
}
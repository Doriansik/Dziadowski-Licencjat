using System;
using UnityEngine;

[Serializable]
public class EnemyMove
{
    private const int DefaultHitCount = 1;
    private const int MinimumHitCount = 1;

    public EnemyIntentType intentType;
    public int value;
    public int hitCount = DefaultHitCount;
    public StatusType statusType;

    public int CalculateEffectiveDamage(StatusManager statusManager)
    {
        if (statusManager == null)
        {
            return value;
        }

        float weaknessMultiplier = statusManager.GetEnemyDamageMultiplier();
        float vulnerabilityMultiplier = statusManager.GetPlayerVulnerabilityMultiplier();
        float finalDamage = value * weaknessMultiplier * vulnerabilityMultiplier;

        return Mathf.RoundToInt(finalDamage);
    }

    public int GetEffectiveHitCount()
    {
        return hitCount >= MinimumHitCount ? hitCount : DefaultHitCount;
    }
}
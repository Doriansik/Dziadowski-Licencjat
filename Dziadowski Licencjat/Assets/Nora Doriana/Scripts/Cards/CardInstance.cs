using UnityEngine;

public class CardInstance
{
    public CardSO BaseCard { get; private set; }
    public int CurrentMana { get; private set; }
    public int CurrentDamage { get; private set; }
    public int CurrentBlock { get; private set; }

    private int bonusDamage;

    private const float BaseMultiplier = 1f;
    private const float PercentageDivisor = 100f;

    public CardInstance(CardSO baseCard)
    {
        BaseCard = baseCard;
        CurrentMana = baseCard.cardMana;
        CurrentDamage = baseCard.cardDamage;
        CurrentBlock = baseCard.cardBlock;

        int defaultBonus = 0;
        bonusDamage = defaultBonus;
    }

    public void AddBonusDamage(int amount)
    {
        bonusDamage += amount;
    }

    public void RecalculateStats(StatusManager statusManager)
    {
        float vulnerabilityMultiplier = BaseMultiplier + (statusManager.EnemyVulnerabilityPercentage / PercentageDivisor);
        float weaknessMultiplier = BaseMultiplier - (statusManager.PlayerWeaknessPercentage / PercentageDivisor);

        float rawDamage = (BaseCard.cardDamage + bonusDamage) * vulnerabilityMultiplier * weaknessMultiplier;
        CurrentDamage = Mathf.RoundToInt(rawDamage);

        CurrentBlock = BaseCard.cardBlock;
        CurrentMana = BaseCard.cardMana;
    }
}
using UnityEngine;

public class CardInstance
{
    public CardSO BaseCard { get; private set; }
    public int CurrentMana { get; private set; }
    public int CurrentDamage { get; private set; }
    public int CurrentBlock { get; private set; }

    private int bonusDamage;

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
        float vulnerabilityMultiplier = statusManager.GetEnemyVulnerabilityMultiplier();
        float weaknessMultiplier = statusManager.GetPlayerDamageMultiplier();

        float rawDamage = (BaseCard.cardDamage + bonusDamage) * vulnerabilityMultiplier * weaknessMultiplier;
        CurrentDamage = Mathf.RoundToInt(rawDamage);

        CurrentBlock = BaseCard.cardBlock;
        CurrentMana = BaseCard.cardMana;
    }
}
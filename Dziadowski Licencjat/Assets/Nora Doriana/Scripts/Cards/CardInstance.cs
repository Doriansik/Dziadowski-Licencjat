using UnityEngine;

public class CardInstance
{
    public CardSO BaseCard { get; private set; }

    public int CurrentDamage { get; private set; }
    public int CurrentBlock { get; private set; }
    public int CurrentMana { get; private set; }

    public int CurrentMinionDamage { get; private set; }
    public int CurrentMinionBlock { get; private set; }

    public CardInstance(CardSO cardData)
    {
        BaseCard = cardData;

        CurrentDamage = cardData.cardDamage;
        CurrentBlock = cardData.cardBlock;
        CurrentMana = cardData.cardMana;

        CurrentMinionDamage = cardData.minionDamage;
        CurrentMinionBlock = cardData.minionBlock;
    }

    public void AddBonusDamage(int amount)
    {
        CurrentDamage += amount;
    }

    public void RecalculateStats(StatusManager statusManager)
    {
        int minimumDamageThreshold = 0;

        if (BaseCard.cardDamage > minimumDamageThreshold)
        {
            float playerDamageMultiplier = statusManager.GetPlayerDamageMultiplier();
            float enemyVulnerabilityMultiplier = statusManager.GetEnemyVulnerabilityMultiplier();

            float totalDamageMultiplier = playerDamageMultiplier * enemyVulnerabilityMultiplier;
            float calculatedDamage = BaseCard.cardDamage * totalDamageMultiplier;

            CurrentDamage = Mathf.RoundToInt(calculatedDamage);
        }
    }
}
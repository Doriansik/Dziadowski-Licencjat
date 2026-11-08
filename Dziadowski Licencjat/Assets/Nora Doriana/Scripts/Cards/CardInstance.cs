public class CardInstance
{
    public CardSO BaseCard { get; private set; }
    public int CurrentMana { get; private set; }
    public float CurrentDamage { get; private set; }
    public int CurrentBlock { get; private set; }

    private float bonusDamage;

    private const float BaseMultiplier = 1f;
    private const float PercentageDivisor = 100f;

    public CardInstance(CardSO baseCard)
    {
        BaseCard = baseCard;
        CurrentMana = baseCard.cardMana;
        CurrentDamage = baseCard.cardDamage;
        CurrentBlock = baseCard.cardBlock;

        float defaultBonus = 0f;
        bonusDamage = defaultBonus;
    }

    public void AddBonusDamage(float amount)
    {
        bonusDamage += amount;
    }

    public void RecalculateStats(StatusManager statusManager)
    {
        float vulnerabilityMultiplier = BaseMultiplier + (statusManager.EnemyVulnerabilityPercentage / PercentageDivisor);

        CurrentDamage = (BaseCard.cardDamage + bonusDamage) * vulnerabilityMultiplier;

        CurrentBlock = BaseCard.cardBlock;
        CurrentMana = BaseCard.cardMana;
    }
}
public class CardInstance
{
    public CardSO BaseCard { get; private set; }
    public int CurrentMana { get; private set; }
    public float CurrentDamage { get; private set; }
    public int CurrentBlock { get; private set; }

    public CardInstance(CardSO baseCard)
    {
        BaseCard = baseCard;
        CurrentMana = baseCard.cardMana;
        CurrentDamage = baseCard.cardDamage;
        CurrentBlock = baseCard.cardBlock;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "Card/CardSO")]
public class CardSO : ScriptableObject
{
    public Sprite cardSprite;
    public CardRarity cardRarity;
    public string cardName;
    [TextArea]
    public string cardDescription;
    public float cardDamage;
    public int cardMana;
    public int cardBlock;
}

public enum CardRarity
{
    Common,
    Rare,
    Epic
}

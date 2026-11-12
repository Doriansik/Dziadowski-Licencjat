using UnityEngine;

public enum CardRarity
{
    Starter,
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "CardSO", menuName = "Card/CardSO")]
public class CardSO : ScriptableObject
{
    public Sprite cardSprite;
    public CardRarity cardRarity;
    public string cardName;
    [TextArea]
    public string cardDescription;
    public int cardDamage;
    public int cardMana;
    public int cardBlock;
    public bool isExhaust;

    public int selfWeaknessTurns;
    public int enemyWeaknessTurns;
    public int enemyVulnerabilityTurns;
    public int summonAmount;
    public int scoopAmount;
    public int discardFromDrawPileAmount;
}
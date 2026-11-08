using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardDamage;
    [SerializeField] private TextMeshProUGUI cardBlock;
    [SerializeField] private TextMeshProUGUI cardMana;
    [SerializeField] private TextMeshProUGUI cardRarity;
    [SerializeField] private Image cardSprite;

    public CardInstance CurrentInstance { get; private set; }

    public void Initialize(CardInstance cardInstance)
    {
        CurrentInstance = cardInstance;

        cardSprite.sprite = cardInstance.BaseCard.cardSprite;
        cardName.text = cardInstance.BaseCard.cardName;
        cardDescription.text = cardInstance.BaseCard.cardDescription;
        cardRarity.text = cardInstance.BaseCard.cardRarity.ToString();

        RefreshStatsVisuals();
    }

    public void RefreshStatsVisuals()
    {
        cardDamage.text = CurrentInstance.CurrentDamage.ToString();
        cardMana.text = CurrentInstance.CurrentMana.ToString();
        cardBlock.text = CurrentInstance.CurrentBlock.ToString();
    }
}
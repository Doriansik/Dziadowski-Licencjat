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

    public void Initialize(CardInstance cardInstance)
    {
        cardSprite.sprite = cardInstance.BaseCard.cardSprite;
        cardName.text = cardInstance.BaseCard.cardName;
        cardDescription.text = cardInstance.BaseCard.cardDescription;
        cardRarity.text = cardInstance.BaseCard.cardRarity.ToString();

        cardDamage.text = cardInstance.CurrentDamage.ToString();
        cardMana.text = cardInstance.CurrentMana.ToString();
        cardBlock.text = cardInstance.CurrentBlock.ToString();
    }
}
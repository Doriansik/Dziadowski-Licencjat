using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : MonoBehaviour
{
    [SerializeField] private CardSO cardSO;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardDamage;
    [SerializeField] private TextMeshProUGUI cardBlock;
    [SerializeField] private TextMeshProUGUI cardMana;
    [SerializeField] private TextMeshProUGUI cardRarity;
    [SerializeField] private Image cardSprite;


    private void Start()
    {
        cardSprite.sprite = cardSO.cardSprite;
        cardName.text = cardSO.cardName;
        cardDescription.text = cardSO.cardDescription;
        cardRarity.text = cardSO.cardRarity.ToString();
        cardDamage.text = cardSO.cardDamage.ToString();
        cardMana.text = cardSO.cardMana.ToString();
        cardBlock.text = cardSO.cardBlock.ToString();
    }
}

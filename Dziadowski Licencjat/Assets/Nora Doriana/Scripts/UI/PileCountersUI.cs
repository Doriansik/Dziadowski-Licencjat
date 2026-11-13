using TMPro;
using UnityEngine;

public class PileCountersUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private TextMeshProUGUI drawPileText;
    [SerializeField] private TextMeshProUGUI discardPileText;
    [SerializeField] private TextMeshProUGUI exhaustPileText;

    private void OnEnable()
    {
        if (cardManager != null)
        {
            cardManager.OnPileSizesChanged += UpdateCounters;
        }
    }

    private void OnDisable()
    {
        if (cardManager != null)
        {
            cardManager.OnPileSizesChanged -= UpdateCounters;
        }
    }

    private void UpdateCounters()
    {
        drawPileText.text = cardManager.DrawPileCount.ToString();
        discardPileText.text = cardManager.DiscardPileCount.ToString();
        exhaustPileText.text = cardManager.ExhaustPileCount.ToString();
    }
}
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
        drawPileText.text = "Draw: " + cardManager.DrawPileCount;
        discardPileText.text = "Discard: " + cardManager.DiscardPileCount;
        exhaustPileText.text = "Exhaust: " + cardManager.ExhaustPileCount;
    }
}
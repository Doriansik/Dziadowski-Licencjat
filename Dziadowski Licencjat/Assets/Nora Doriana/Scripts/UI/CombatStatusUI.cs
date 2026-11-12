using TMPro;
using UnityEngine;

public class CombatStatusUI : MonoBehaviour
{
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private TextMeshProUGUI playerStatusText;
    [SerializeField] private TextMeshProUGUI enemyStatusText;

    private void OnEnable()
    {
        if (statusManager != null)
        {
            statusManager.OnStatusesChanged += UpdateStatusTexts;
        }
    }

    private void OnDisable()
    {
        if (statusManager != null)
        {
            statusManager.OnStatusesChanged -= UpdateStatusTexts;
        }
    }

    private void UpdateStatusTexts()
    {
        playerStatusText.text = FormatStatus("Weakness", statusManager.PlayerWeaknessTurns) +
                                FormatStatus("Vulnerability", statusManager.PlayerVulnerabilityTurns);

        enemyStatusText.text = FormatStatus("Weakness", statusManager.EnemyWeaknessTurns) +
                               FormatStatus("Vulnerability", statusManager.EnemyVulnerabilityTurns);
    }

    private string FormatStatus(string statusName, int turnsValue)
    {
        int minimumValue = 0;

        if (turnsValue > minimumValue)
        {
            return statusName + ": " + turnsValue + "\n";
        }

        return string.Empty;
    }
}
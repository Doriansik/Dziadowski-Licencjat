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
        playerStatusText.text = FormatStatus("Weakness", statusManager.PlayerWeaknessPercentage) +
                                FormatStatus("Vulnerability", statusManager.PlayerVulnerabilityPercentage);

        enemyStatusText.text = FormatStatus("Weakness", statusManager.EnemyWeaknessPercentage) +
                               FormatStatus("Vulnerability", statusManager.EnemyVulnerabilityPercentage);
    }

    private string FormatStatus(string statusName, int percentageValue)
    {
        int minimumValue = 0;

        if (percentageValue > minimumValue)
        {
            return statusName + ": " + percentageValue + "%\n";
        }

        return string.Empty;
    }
}
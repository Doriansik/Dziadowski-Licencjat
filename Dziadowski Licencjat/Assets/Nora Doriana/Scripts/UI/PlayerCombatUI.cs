using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatUI : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private GameObject blockContainer;

    private void OnEnable()
    {
        if (playerManager != null)
        {
            playerManager.OnHealthChanged += UpdateHealthUI;
            playerManager.OnBlockChanged += UpdateBlockUI;
        }
    }

    private void OnDisable()
    {
        if (playerManager != null)
        {
            playerManager.OnHealthChanged -= UpdateHealthUI;
            playerManager.OnBlockChanged -= UpdateBlockUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }

    private void UpdateBlockUI(int currentBlock)
    {
        int emptyBlock = 0;

        if (currentBlock > emptyBlock)
        {
            blockContainer.SetActive(true);
            blockText.text = currentBlock.ToString();
        }
        else
        {
            blockContainer.SetActive(false);
        }
    }
}
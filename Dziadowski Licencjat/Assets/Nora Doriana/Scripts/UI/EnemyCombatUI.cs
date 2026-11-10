using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatUI : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private GameObject blockContainer;
    [SerializeField] private TextMeshProUGUI intentText;

    private EnemyInstance currentEnemy;

    private void OnEnable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned += HandleEnemySpawned;
        }
    }

    private void OnDisable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned -= HandleEnemySpawned;
        }

        UnsubscribeFromEnemy();
    }

    private void HandleEnemySpawned(EnemyInstance enemy)
    {
        UnsubscribeFromEnemy();

        currentEnemy = enemy;
        currentEnemy.OnHealthChanged += UpdateHealthUI;
        currentEnemy.OnBlockChanged += UpdateBlockUI;
        currentEnemy.OnIntentChanged += UpdateIntentUI;

        UpdateHealthUI(currentEnemy.CurrentHealth, currentEnemy.BaseEnemy.maxHealth);
        UpdateBlockUI(currentEnemy.CurrentBlock);
    }

    private void UnsubscribeFromEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnHealthChanged -= UpdateHealthUI;
            currentEnemy.OnBlockChanged -= UpdateBlockUI;
            currentEnemy.OnIntentChanged -= UpdateIntentUI;
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

    private void UpdateIntentUI(EnemyMove intent)
    {
        intentText.text = intent.intentType.ToString() + " " + intent.value;
    }
}
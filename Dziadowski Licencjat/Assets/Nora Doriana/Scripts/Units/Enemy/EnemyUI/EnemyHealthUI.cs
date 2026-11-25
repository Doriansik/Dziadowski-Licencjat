using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

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

        UpdateHealthUI(currentEnemy.CurrentHealth, currentEnemy.BaseEnemy.maxHealth);
    }

    private void UnsubscribeFromEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnHealthChanged -= UpdateHealthUI;
            currentEnemy = null;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }
}
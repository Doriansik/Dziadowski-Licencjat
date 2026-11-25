using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatUI : MonoBehaviour
{
    private const int EmptyBlock = 0;
    private const int MinimumHitCount = 1;
    private const int DefaultHitCount = 1;
    private const int SingleHitCount = 1;
    private const int MinimumDisplayValue = 0;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private GameObject blockContainer;

    [SerializeField] private GameObject intentContainer;
    [SerializeField] private Image intentIcon;
    [SerializeField] private TextMeshProUGUI intentText;

    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite defendSprite;
    [SerializeField] private Sprite debuffSprite;
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite specialAttackSprite;

    private EnemyInstance currentEnemy;

    private void OnEnable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned += HandleEnemySpawned;
        }

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged += RefreshIntentUI;
        }
    }

    private void OnDisable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned -= HandleEnemySpawned;
        }

        if (statusManager != null)
        {
            statusManager.OnStatusesChanged -= RefreshIntentUI;
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
        currentEnemy.OnDied += HandleEnemyDied;

        UpdateHealthUI(currentEnemy.CurrentHealth, currentEnemy.BaseEnemy.maxHealth);
        UpdateBlockUI(currentEnemy.CurrentBlock);

        if (currentEnemy.CurrentMove != null)
        {
            UpdateIntentUI(currentEnemy.CurrentMove);
        }
    }

    private void UnsubscribeFromEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnHealthChanged -= UpdateHealthUI;
            currentEnemy.OnBlockChanged -= UpdateBlockUI;
            currentEnemy.OnIntentChanged -= UpdateIntentUI;
            currentEnemy.OnDied -= HandleEnemyDied;
        }
    }

    private void HandleEnemyDied()
    {
        if (intentContainer != null)
        {
            intentContainer.SetActive(false);
        }
    }

    private void RefreshIntentUI()
    {
        if (currentEnemy != null && currentEnemy.CurrentMove != null)
        {
            UpdateIntentUI(currentEnemy.CurrentMove);
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
        if (currentBlock > EmptyBlock)
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
        if (intent == null)
        {
            if (intentContainer != null)
            {
                intentContainer.SetActive(false);
            }
            return;
        }

        if (intentContainer != null)
        {
            intentContainer.SetActive(true);
        }

        if (intentIcon != null)
        {
            intentIcon.sprite = GetSpriteForIntent(intent.intentType);
        }

        intentText.text = FormatIntentValue(intent);
    }

    private Sprite GetSpriteForIntent(EnemyIntentType intentType)
    {
        if (intentType == EnemyIntentType.Attack)
        {
            return attackSprite;
        }

        if (intentType == EnemyIntentType.Defend)
        {
            return defendSprite;
        }

        if (intentType == EnemyIntentType.Heal)
        {
            return healSprite;
        }

        if (intentType == EnemyIntentType.SpecialAttack)
        {
            return specialAttackSprite;
        }

        return debuffSprite;
    }

    private string FormatIntentValue(EnemyMove intent)
    {
        if (intent.intentType == EnemyIntentType.Attack || intent.intentType == EnemyIntentType.SpecialAttack)
        {
            int effectiveDamage = CalculateEffectiveDamage(intent.value);
            int hits = intent.hitCount >= MinimumHitCount ? intent.hitCount : DefaultHitCount;

            if (hits > SingleHitCount)
            {
                return effectiveDamage + "x" + hits;
            }

            return effectiveDamage.ToString();
        }

        if (intent.value > MinimumDisplayValue)
        {
            return intent.value.ToString();
        }

        return string.Empty;
    }

    private int CalculateEffectiveDamage(int baseDamage)
    {
        if (statusManager == null)
        {
            return baseDamage;
        }

        float weaknessMultiplier = statusManager.GetEnemyDamageMultiplier();
        float vulnerabilityMultiplier = statusManager.GetPlayerVulnerabilityMultiplier();
        return Mathf.RoundToInt(baseDamage * weaknessMultiplier * vulnerabilityMultiplier);
    }
}
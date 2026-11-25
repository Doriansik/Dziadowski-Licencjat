using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct IntentIconEntry
{
    public EnemyIntentType intentType;
    public Sprite icon;
}

public class EnemyIntentUI : MonoBehaviour
{
    private const int SingleHitCount = 1;
    private const int MinimumDisplayValue = 0;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private GameObject intentContainer;
    [SerializeField] private Image intentIcon;
    [SerializeField] private TextMeshProUGUI intentText;
    [SerializeField] private List<IntentIconEntry> intentIcons;

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
        currentEnemy.OnIntentChanged += UpdateIntentUI;
        currentEnemy.OnDied += HandleEnemyDied;

        if (currentEnemy.CurrentMove != null)
        {
            UpdateIntentUI(currentEnemy.CurrentMove);
        }
    }

    private void UnsubscribeFromEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnIntentChanged -= UpdateIntentUI;
            currentEnemy.OnDied -= HandleEnemyDied;
            currentEnemy = null;
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

    private void UpdateIntentUI(EnemyMove intent)
    {
        if (intent == null)
        {
            HandleEnemyDied();
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
        if (intentIcons == null)
        {
            return null;
        }

        foreach (IntentIconEntry entry in intentIcons)
        {
            if (entry.intentType == intentType)
            {
                return entry.icon;
            }
        }

        return null;
    }

    private string FormatIntentValue(EnemyMove intent)
    {
        if (intent.intentType == EnemyIntentType.Attack || intent.intentType == EnemyIntentType.SpecialAttack)
        {
            int effectiveDamage = intent.CalculateEffectiveDamage(statusManager);
            int hits = intent.GetEffectiveHitCount();

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
}
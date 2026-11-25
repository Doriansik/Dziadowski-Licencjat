using TMPro;
using UnityEngine;

public class EnemyBlockUI : MonoBehaviour
{
    private const int EmptyBlock = 0;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private GameObject blockContainer;

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
        currentEnemy.OnBlockChanged += UpdateBlockUI;

        UpdateBlockUI(currentEnemy.CurrentBlock);
    }

    private void UnsubscribeFromEnemy()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnBlockChanged -= UpdateBlockUI;
            currentEnemy = null;
        }
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
}
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Image enemyImage;

    private void OnEnable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned += UpdateEnemySprite;
        }
    }

    private void OnDisable()
    {
        if (enemyManager != null)
        {
            enemyManager.OnEnemySpawned -= UpdateEnemySprite;
        }
    }

    private void UpdateEnemySprite(EnemyInstance enemyInstance)
    {
        enemyImage.sprite = enemyInstance.BaseEnemy.enemySprite;
        enemyImage.SetNativeSize();
    }
}
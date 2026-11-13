using TMPro;
using UnityEngine;

public class MinionVisual : MonoBehaviour
{
    [SerializeField] private MinionManager minionManager;
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI blockText;

    private void OnEnable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned += ShowMinion;
            minionManager.OnDied += HideMinion;
            minionManager.OnHealthChanged += UpdateHealth;
            minionManager.OnBlockChanged += UpdateBlock;
        }
    }

    private void OnDisable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned -= ShowMinion;
            minionManager.OnDied -= HideMinion;
            minionManager.OnHealthChanged -= UpdateHealth;
            minionManager.OnBlockChanged -= UpdateBlock;
        }
    }

    private void Start()
    {
        HideMinion();
    }

    private void ShowMinion()
    {
        visualContainer.SetActive(true);
    }

    private void HideMinion()
    {
        visualContainer.SetActive(false);
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        hpText.text = currentHealth + " / " + maxHealth;
    }

    private void UpdateBlock(int currentBlock)
    {
        blockText.text = currentBlock.ToString();
    }
}
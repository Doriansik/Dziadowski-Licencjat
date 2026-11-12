using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionVisual : MonoBehaviour
{
    [SerializeField] private MinionManager minionManager;
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private TextMeshProUGUI hpText;

    private void OnEnable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned += ShowMinion;
            minionManager.OnDied += HideMinion;
            minionManager.OnHealthChanged += UpdateHealth;
        }
    }

    private void OnDisable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned -= ShowMinion;
            minionManager.OnDied -= HideMinion;
            minionManager.OnHealthChanged -= UpdateHealth;
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
}
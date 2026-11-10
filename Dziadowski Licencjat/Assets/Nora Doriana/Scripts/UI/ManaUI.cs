using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private TextMeshProUGUI manaText;

    private void OnEnable()
    {
        if (manaManager != null)
        {
            manaManager.OnManaChanged += UpdateManaText;
        }
    }

    private void OnDisable()
    {
        if (manaManager != null)
        {
            manaManager.OnManaChanged -= UpdateManaText;
        }
    }

    private void UpdateManaText(int currentMana, int maxMana)
    {
        manaText.text = currentMana + " / " + maxMana;
    }
}
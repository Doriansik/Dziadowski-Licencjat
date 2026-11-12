using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Button victoryContinueButton;
    [SerializeField] private Button defeatRetryButton;

    private void OnEnable()
    {
        if (combatManager != null)
        {
            combatManager.OnCombatVictory += ShowVictoryScreen;
            combatManager.OnCombatDefeat += ShowDefeatScreen;
        }

        if (victoryContinueButton != null)
        {
            victoryContinueButton.onClick.AddListener(ReloadCurrentScene);
        }

        if (defeatRetryButton != null)
        {
            defeatRetryButton.onClick.AddListener(ReloadCurrentScene);
        }
    }

    private void OnDisable()
    {
        if (combatManager != null)
        {
            combatManager.OnCombatVictory -= ShowVictoryScreen;
            combatManager.OnCombatDefeat -= ShowDefeatScreen;
        }

        if (victoryContinueButton != null)
        {
            victoryContinueButton.onClick.RemoveListener(ReloadCurrentScene);
        }

        if (defeatRetryButton != null)
        {
            defeatRetryButton.onClick.RemoveListener(ReloadCurrentScene);
        }
    }

    private void Start()
    {
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    private void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);
    }

    private void ShowDefeatScreen()
    {
        defeatPanel.SetActive(true);
    }

    private void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
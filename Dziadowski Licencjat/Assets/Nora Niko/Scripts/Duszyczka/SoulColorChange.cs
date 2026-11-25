using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoulColorChange : MonoBehaviour
{
    public enum SoulStan
    {
        Attack,
        Defense,
        Heal,
        Idle
    }

    [SerializeField] private MinionManager minionManager;
    [SerializeField] private Image soulImage;

    public SoulStan currentStan = SoulStan.Idle;

    private int lastKnownHealth;
    private int lastKnownBlock;

    private void OnEnable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned += HandleSpawned;
            minionManager.OnBlockChanged += HandleBlockChanged;
            minionManager.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (minionManager != null)
        {
            minionManager.OnSpawned -= HandleSpawned;
            minionManager.OnBlockChanged -= HandleBlockChanged;
            minionManager.OnHealthChanged -= HandleHealthChanged;
        }
    }

    void Start()
    {
        if (soulImage == null)
        {
            soulImage = GetComponent<Image>();
        }

        SetStan(SoulStan.Idle);
    }

    private void HandleSpawned()
    {
        lastKnownHealth = minionManager.CurrentHealth;
        lastKnownBlock = minionManager.CurrentBlock;
        SetStan(SoulStan.Idle);
    }

    private void HandleBlockChanged(int newBlock)
    {
        //jesli tarcza wzrasta -> defense stan (niebieski)
        if (newBlock > lastKnownBlock)
        {
            SetDefenseStan();
        }
        lastKnownBlock = newBlock;
    }

    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        //jesli zycie wzrostlo -> heal stan (zielony)
        if (currentHealth > lastKnownHealth)
        {
            SetHealStan();
        }
        lastKnownHealth = currentHealth;
    }

    public void SetStan(SoulStan stan)
    {
        currentStan = stan;

        soulImage.DOKill();

        if (currentStan == SoulStan.Idle)
        {
            soulImage.DOColor(Color.white, 0.5f);
        }

        if (currentStan == SoulStan.Attack)
        {
            soulImage.DOColor(Color.red, 0.5f);
        }

        if (currentStan == SoulStan.Defense)
        {
            soulImage.DOColor(Color.blue, 0.5f);
        }

        if (currentStan == SoulStan.Heal)
        {
            soulImage.DOColor(Color.green, 0.5f);
        }

    }

    public void SetAttackStan()
    {
        SetStan(SoulStan.Attack);
        Debug.Log("nowy stan duszyczki:" +  currentStan);
    }

    public void SetDefenseStan()
    {
        SetStan(SoulStan.Defense);
        Debug.Log("nowy stan duszyczki:" + currentStan);
    }

    public void SetIdleStan()
    {
        SetStan(SoulStan.Idle);
        Debug.Log("nowy stan duszyczki:" + currentStan);
    }

    public void SetHealStan()
    {
        SetStan(SoulStan.Heal);
        Debug.Log("nowy stan duszyczki:" + currentStan);
    }
}

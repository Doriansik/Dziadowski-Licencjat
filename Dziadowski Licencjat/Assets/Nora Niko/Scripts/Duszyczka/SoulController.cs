using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoulController : MonoBehaviour
{
    public enum SoulStan
    {
        Attack,
        Defense,
        Heal,
        Idle
    }

    public SoulStan currentStan = SoulStan.Idle;
    private Image soulImage;

    void Start()
    {
        soulImage = GetComponent<Image>();

        SetStan(SoulStan.Idle);
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

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayZone : MonoBehaviour, IDropHandler
{
    public event Action<CardInstance> OnCardDropped;

    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private CardSO saltCardData;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            CardInteraction cardInteraction = eventData.pointerDrag.GetComponent<CardInteraction>();

            if (cardInteraction != null)
            {
                CardInstance cardInstance = cardInteraction.GetCardInstance();

                if (manaManager.TrySpendMana(cardInstance.CurrentMana))
                {
                    GenerateCardActions(cardInstance);

                    cardInteraction.MarkAsPlayed();
                    OnCardDropped?.Invoke(cardInstance);
                }
            }
        }
    }

    private void GenerateCardActions(CardInstance cardInstance)
    {
        float damageToDeal = cardInstance.CurrentDamage;
        int minimumThreshold = 0;

        if (damageToDeal > minimumThreshold)
        {
            DamageAction damageAction = new DamageAction(damageToDeal);
            actionQueueManager.AddAction(damageAction);
        }

        if (cardInstance.BaseCard.selfWeaknessAmount > minimumThreshold)
        {
            ApplyWeaknessAction weaknessAction = new ApplyWeaknessAction(statusManager, cardInstance.BaseCard.selfWeaknessAmount);
            actionQueueManager.AddAction(weaknessAction);
        }

        if (cardInstance.BaseCard.summonAmount > minimumThreshold)
        {
            SummonAction summonAction = new SummonAction(cardInstance.BaseCard.summonAmount);
            actionQueueManager.AddAction(summonAction);
        }

        if (cardInstance.BaseCard.scoopAmount > minimumThreshold)
        {
            ScoopAction scoopAction = new ScoopAction(cardManager, saltCardData, cardInstance.BaseCard.scoopAmount);
            actionQueueManager.AddAction(scoopAction);
        }
    }
}
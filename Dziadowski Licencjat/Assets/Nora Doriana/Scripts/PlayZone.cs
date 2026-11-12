using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayZone : MonoBehaviour, IDropHandler
{
    public event Action<CardInstance> OnCardDropped;

    [SerializeField] private CombatManager combatManager;
    [SerializeField] private ActionQueueManager actionQueueManager;
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private ManaManager manaManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MinionManager minionManager;
    [SerializeField] private CardSO saltCardData;

    public void OnDrop(PointerEventData eventData)
    {
        if (combatManager.CurrentState != CombatState.PlayerTurn)
        {
            return;
        }

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
        int damageToDeal = cardInstance.CurrentDamage;
        int minimumThreshold = 0;

        if (damageToDeal > minimumThreshold)
        {
            DamageAction damageAction = new DamageAction(damageToDeal, enemyManager);
            actionQueueManager.AddAction(damageAction);
        }

        int blockToGain = cardInstance.CurrentBlock;

        if (blockToGain > minimumThreshold)
        {
            BlockAction blockAction = new BlockAction(blockToGain, playerManager);
            actionQueueManager.AddAction(blockAction);
        }

        if (cardInstance.BaseCard.selfWeaknessTurns > minimumThreshold)
        {
            ApplyWeaknessAction weaknessAction = new ApplyWeaknessAction(statusManager, cardInstance.BaseCard.selfWeaknessTurns);
            actionQueueManager.AddAction(weaknessAction);
        }

        if (cardInstance.BaseCard.enemyWeaknessTurns > minimumThreshold)
        {
            ApplyEnemyWeaknessAction enemyWeaknessAction = new ApplyEnemyWeaknessAction(statusManager, cardInstance.BaseCard.enemyWeaknessTurns);
            actionQueueManager.AddAction(enemyWeaknessAction);
        }

        if (cardInstance.BaseCard.enemyVulnerabilityTurns > minimumThreshold)
        {
            ApplyEnemyVulnerabilityAction enemyVulnerabilityAction = new ApplyEnemyVulnerabilityAction(statusManager, cardInstance.BaseCard.enemyVulnerabilityTurns);
            actionQueueManager.AddAction(enemyVulnerabilityAction);
        }

        if (cardInstance.BaseCard.summonAmount > minimumThreshold)
        {
            SummonAction summonAction = new SummonAction(minionManager, cardInstance.BaseCard.summonAmount);
            actionQueueManager.AddAction(summonAction);
        }

        if (cardInstance.BaseCard.scoopAmount > minimumThreshold)
        {
            ScoopAction scoopAction = new ScoopAction(cardManager, saltCardData, cardInstance.BaseCard.scoopAmount);
            actionQueueManager.AddAction(scoopAction);
        }

        if (cardInstance.BaseCard.discardFromDrawPileAmount > minimumThreshold)
        {
            DiscardFromDrawPileAction discardAction = new DiscardFromDrawPileAction(cardManager, cardInstance.BaseCard.discardFromDrawPileAmount);
            actionQueueManager.AddAction(discardAction);
        }
    }
}
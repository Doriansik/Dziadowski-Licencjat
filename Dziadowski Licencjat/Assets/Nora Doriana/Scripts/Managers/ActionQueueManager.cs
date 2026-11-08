using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    private Queue<IGameAction> actionQueue = new Queue<IGameAction>();
    private IGameAction currentAction;

    public void AddAction(IGameAction action)
    {
        actionQueue.Enqueue(action);
    }

    private void Update()
    {
        if (currentAction != null)
        {
            if (currentAction.IsCompleted)
            {
                currentAction = null;
            }
            else
            {
                currentAction.UpdateAction();
            }
        }

        if (currentAction == null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            currentAction.Execute();
        }
    }
}
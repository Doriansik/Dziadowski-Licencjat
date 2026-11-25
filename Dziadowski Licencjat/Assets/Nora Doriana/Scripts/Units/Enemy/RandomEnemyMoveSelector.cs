using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyMoveSelector : IEnemyMoveSelector
{
    private const int EmptyCount = 0;
    private const int SingleMoveCount = 1;
    private const int FirstIndex = 0;
    private const int MaxConsecutiveRepeats = 2;

    public EnemyMove SelectNextMove(EnemyInstance enemy)
    {
        List<EnemyMove> moves = enemy.BaseEnemy.possibleMoves;

        if (moves == null || moves.Count == EmptyCount)
        {
            return null;
        }

        if (moves.Count == SingleMoveCount)
        {
            return moves[FirstIndex];
        }

        List<EnemyMove> availableMoves = new List<EnemyMove>(moves);

        if (enemy.PreviousMove != null && enemy.ConsecutiveSameMoveCount >= MaxConsecutiveRepeats)
        {
            availableMoves.Remove(enemy.PreviousMove);
        }

        if (availableMoves.Count == EmptyCount)
        {
            availableMoves = moves;
        }

        int randomIndex = Random.Range(FirstIndex, availableMoves.Count);
        return availableMoves[randomIndex];
    }
}
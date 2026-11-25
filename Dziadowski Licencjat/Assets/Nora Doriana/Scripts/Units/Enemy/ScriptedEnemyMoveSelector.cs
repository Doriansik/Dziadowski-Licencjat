using System.Collections.Generic;

public class ScriptedEnemyMoveSelector : IEnemyMoveSelector
{
    private const int EmptyCount = 0;

    public EnemyMove SelectNextMove(EnemyInstance enemy)
    {
        List<EnemyMove> moves = enemy.BaseEnemy.possibleMoves;

        if (moves == null || moves.Count == EmptyCount)
        {
            return null;
        }

        EnemyMove selectedMove = moves[enemy.MoveSequenceIndex];
        enemy.AdvanceSequenceIndex(moves.Count);
        return selectedMove;
    }
}
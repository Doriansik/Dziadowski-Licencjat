using System.Collections.Generic;
using UnityEngine;

public enum EnemyTier
{
    Normal,
    Elite,
    Boss
}

public enum EnemyAIMode
{
    Random,
    ScriptedSequence
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public EnemyTier enemyTier;
    public EnemyAIMode AIMode;
    public int maxHealth;
    public List<EnemyMove> possibleMoves;
}
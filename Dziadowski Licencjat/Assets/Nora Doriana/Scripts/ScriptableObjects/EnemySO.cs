using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemy/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    public int maxHealth;
    public List<EnemyMove> possibleMoves;
}
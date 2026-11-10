using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "NewNodeBlueprint", menuName = "Map/NodeBlueprint")]
    public class MapNodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public NodeType nodeType;
    }
}
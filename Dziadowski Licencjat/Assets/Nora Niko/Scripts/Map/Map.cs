using System.Collections.Generic;
using System.Linq;

namespace Map
{
    public class Map
    {
        public List<Node> nodes;
        public List<Point> path;
        public string bossNodeName;

        public Map(List<Node> nodes, List<Point> path)
        {
            this.nodes = nodes;
            this.path = path;
        }

        public Node GetNode(Point point)
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }

        public float DistanceBetweenFirstAndLastLayers()
        {
            var bossNode = nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
            var firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);
            if (bossNode == null || firstLayerNode == null) return 0f;
            return bossNode.position.y - firstLayerNode.position.y;
        }

        public List<Node> GetNodesOnFloor(int floor)
        {
            return nodes.Where(n => n.point.y == floor).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [Serializable]
    public class Point : IEquatable<Point>
    {
        public int x; //kolumna (lane)
        public int y; //pietro (floor / layer)

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Point other)
        {
            if (other is null) return false;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj) => Equals(obj as Point);
        public override int GetHashCode() => HashCode.Combine(x, y);
    }

    [Serializable]
    public class Node
    {
        public Point point;
        public NodeType nodeType;
        public NodeStateType state = NodeStateType.Locked;
        public Vector2 position;

        //identyfikatory polaczen oparte o wspolrzedne Point
        public HashSet<Point> incoming = new();
        public HashSet<Point> outgoing = new();

        public Node(NodeType nodeType, Point point)
        {
            this.nodeType = nodeType;
            this.point = point;
        }

        public void AddIncoming(Point p) => incoming.Add(p);
        public void AddOutgoing(Point p) => outgoing.Add(p);
    }
}
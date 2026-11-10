using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILine : Graphic
    {
        public Vector2 from;
        public Vector2 to;
        public float thickness = 4f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 dir = (to - from).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x) * (thickness / 2f);

            UIVertex v1 = UIVertex.simpleVert;
            v1.color = color;
            v1.position = from - normal;

            UIVertex v2 = UIVertex.simpleVert;
            v2.color = color;
            v2.position = from + normal;

            UIVertex v3 = UIVertex.simpleVert;
            v3.color = color;
            v3.position = to + normal;

            UIVertex v4 = UIVertex.simpleVert;
            v4.color = color;
            v4.position = to - normal;

            vh.AddUIVertexQuad(new UIVertex[] { v1, v2, v3, v4 });
        }

        public void SetPoints(Vector2 start, Vector2 end)
        {
            from = start;
            to = end;
            SetVerticesDirty();
        }
    }
}
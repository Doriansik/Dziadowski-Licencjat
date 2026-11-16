using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapNodeView : MonoBehaviour
    {
        [Header("Referencje UI")]
        [SerializeField] private Image bgImage;   //glowne kołko (tło pokoju)
        [SerializeField] private Image iconImage; //dziecko Room_Icon (ikona pokoju)
        [SerializeField] private Button button;

        public Node Node { get; private set; }
        private MapView mapView;

        public void Initialize(Node node, MapNodeBlueprint blueprint, MapView view)
        {
            this.Node = node;
            this.mapView = view;

            if (iconImage != null && blueprint != null)
            {
                iconImage.sprite = blueprint.sprite;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);

            SetState(node.state);
        }

        public void SetState(NodeStateType state)
        {
            Node.state = state;
            switch (state)
            {
                case NodeStateType.Locked: //pokoje PRZED nami / niedostepne
                    button.interactable = false;
                    if (bgImage != null) bgImage.color = new Color(0.75f, 0.75f, 0.75f, 0.6f);
                    if (iconImage != null) iconImage.color = new Color(1f, 1f, 1f, 0.8f);
                    transform.localScale = Vector3.one;
                    break;

                case NodeStateType.Attainable: //aktywne pokoje DO WYBORU przez gracza
                    button.interactable = true;
                    if (bgImage != null) bgImage.color = Color.white;
                    if (iconImage != null) iconImage.color = Color.white;
                    transform.localScale = Vector3.one * 1.15f; //lekkie powiekszenie ikonki
                    break;

                case NodeStateType.Visited: //pokoje za nami (odwiedzone)
                    button.interactable = false;
                    if (bgImage != null) bgImage.color = new Color(0.55f, 0.55f, 0.55f, 0.85f);
                    if (iconImage != null) iconImage.color = new Color(0.6f, 0.6f, 0.6f, 0.9f);
                    transform.localScale = Vector3.one;
                    break;
            }
        }

        private void OnClick()
        {
            mapView.OnNodeSelected(this);
        }
    }
}
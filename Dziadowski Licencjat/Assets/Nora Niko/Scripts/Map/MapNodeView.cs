using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapNodeView : MonoBehaviour
    {
        [SerializeField] private Image bgImage;        //główne kółko
        [SerializeField] private Image iconImage;      //dziecko Room_Icon
        [SerializeField] private Image circleHighlight;//dziecko Highlight
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
                case NodeStateType.Locked:
                    if (circleHighlight != null) circleHighlight.gameObject.SetActive(false);
                    button.interactable = false;
                    if (bgImage != null) bgImage.color = new Color(0.25f, 0.25f, 0.25f, 0.7f); //szary zablokowany
                    if (iconImage != null) iconImage.color = new Color(0.6f, 0.6f, 0.6f, 0.5f);
                    break;

                case NodeStateType.Attainable:
                    if (circleHighlight != null)
                    {
                        circleHighlight.gameObject.SetActive(true);
                        circleHighlight.color = Color.white; //biały obrys (kontrast do czarnego tła)
                    }
                    button.interactable = true;
                    if (bgImage != null) bgImage.color = Color.black; //czarne tło aktywnego pokoju
                    if (iconImage != null) iconImage.color = Color.white;
                    break;

                case NodeStateType.Visited:
                    if (circleHighlight != null)
                    {
                        circleHighlight.gameObject.SetActive(true);
                        circleHighlight.color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                    }
                    button.interactable = false;
                    if (bgImage != null) bgImage.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);
                    if (iconImage != null) iconImage.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
                    break;
            }
        }

        private void OnClick()
        {
            mapView.OnNodeSelected(this);
        }
    }
}
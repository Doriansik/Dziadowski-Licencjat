using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapView : MonoBehaviour
    {
        [Header("Konfiguracja")]
        [SerializeField] private MapConfigSO config;
        [SerializeField] private List<MapNodeBlueprint> blueprints;

        [Header("Prefaby UI")]
        [SerializeField] private MapNodeView nodePrefab;
        [SerializeField] private UILine linePrefab;

        [Header("Kontenery UI")]
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private ScrollRect scrollRect;

        private Map currentMap;
        private readonly List<MapNodeView> nodeViews = new();
        private readonly List<UILine> lineViews = new();

        private void Start()
        {
            ShowMap();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartMap();
            }
        }

        //!!!! DO TESTOW GENEROWANIA MAPY !!!!
        public void RestartMap()
        {
            //resetuje stan gracza w singletonie
            if (RunManager.Instance != null)
            {
                RunManager.Instance.ResetRun();
            }

            //generuje mape na nowo i resetuje widok na dolne pietro
            ShowMap();
        }

        public void ShowMap()
        {
            ClearView();

            currentMap = MapGenerator.GetMap(config);
            if (currentMap == null) return;

            //zwieksza wysokosc contentu aby zmiescily sie wszystkie pietra
            float totalHeight = (config.totalFloors + 2) * config.ySpacing;
            contentRoot.sizeDelta = new Vector2(contentRoot.sizeDelta.x, totalHeight);

            //najpierw tworzy wszystkie pokoje i zapisuje je w slowniku
            Dictionary<Point, MapNodeView> nodeMap = new Dictionary<Point, MapNodeView>();

            foreach (var node in currentMap.nodes)
            {
                var view = Instantiate(nodePrefab, contentRoot);

                //ustawia pozycje pokoju
                view.transform.localPosition = new Vector2(node.position.x, node.position.y + 100f);

                var bp = blueprints.FirstOrDefault(b => b.nodeType == node.nodeType);
                view.Initialize(node, bp, this);

                nodeViews.Add(view);
                nodeMap[node.point] = view;
            }

            //tworzy linie korzystajac DOKŁADNIE z faktycznych pozycji kółek
            foreach (var node in currentMap.nodes)
            {
                if (!nodeMap.TryGetValue(node.point, out var fromView)) continue;

                foreach (var outPt in node.outgoing)
                {
                    if (nodeMap.TryGetValue(outPt, out var toView))
                    {
                        var line = Instantiate(linePrefab, contentRoot);

                        //zeruje pozycje i skale obiektu linii
                        line.transform.localPosition = Vector3.zero;
                        line.transform.localScale = Vector3.one;
                        line.transform.SetAsFirstSibling(); //pod kółka

                        //bierze dokladnie lokalne pozycje kółek w Contencie
                        Vector2 startPos = fromView.transform.localPosition;
                        Vector2 endPos = toView.transform.localPosition;

                        line.SetPoints(startPos, endPos);
                        lineViews.Add(line);
                    }
                }
            }

            InitializePlayerPosition();
            StartCoroutine(ScrollToBottomRoutine());
        }

        private IEnumerator ScrollToBottomRoutine()
        {
            //czeka na koniec klatki az Canvas przeliczy pozycje obiektow
            yield return new WaitForEndOfFrame();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 0f; //0 = dół, 1 = góra
            }
        }

        private void InitializePlayerPosition()
        {
            //jesli gracz jest na poczatku gry: odblokuj pietro 0
            foreach (var nodeView in nodeViews.Where(v => v.Node.point.y == 0))
            {
                nodeView.SetState(NodeStateType.Attainable);
            }
        }

        public void OnNodeSelected(MapNodeView selectedView)
        {
            //zapis do RunManagera
            RunManager.Instance.EnterNode(selectedView.Node);

            //zablokuj wszystkie obecne wezly
            foreach (var view in nodeViews)
            {
                if (view.Node.state == NodeStateType.Attainable)
                    view.SetState(NodeStateType.Locked);
            }

            //oznacza obecny pokoj jako odwiedzony
            selectedView.SetState(NodeStateType.Visited);

            //odblokuj tylko bezposrednie pokoje
            foreach (var outPoint in selectedView.Node.outgoing)
            {
                var nextView = nodeViews.FirstOrDefault(v => v.Node.point.Equals(outPoint));
                if (nextView != null)
                {
                    nextView.SetState(NodeStateType.Attainable);
                }
            }
        }

        private void ClearView()
        {
            foreach (var nv in nodeViews) Destroy(nv.gameObject);
            foreach (var lv in lineViews) Destroy(lv.gameObject);
            nodeViews.Clear();
            lineViews.Clear();
        }
    }
}
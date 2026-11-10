using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public static class MapGenerator
    {
        private static MapConfigSO config;

        public static Map GetMap(MapConfigSO conf)
        {
            config = conf;
            //500 prob na wygenerowanie mapy
            int safetyLimit = 500;

            while (safetyLimit-- > 0)
            {
                var candidateMap = GenerateCandidate();
                if (ValidateCandidate(candidateMap))
                {
                    return candidateMap;
                }
            }

            Debug.LogError("[MapGenerator] nie udało się wygenerować mapy spełniającej wszystkie warunki.");
            return null;
        }

        private static Map GenerateCandidate()
        {
            List<Node> nodes = new();
            List<List<Point>> paths = new();

            //definiuje punkty krańcowe
            int centerLane = config.gridWidth / 2;
            Point startPoint = new Point(centerLane, 0); //TYLKO JEDEN START (piętro 0)
            Point bossPoint = new Point(centerLane, config.totalFloors - 1); //TYLKO JEDEN BOSS

            //PIĘTRO 1: wybiera 2-3 sciezki na ktore rozchodzi sie start
            int branchCount = Mathf.Clamp(config.numOfStartingNodes, 2, config.gridWidth);
            List<int> firstFloorLanes = GetRandomLanes(branchCount, config.gridWidth);

            //jesli wylosowane sciezki sa za daleko od środka, wyrownuje je blisko centrum
            for (int i = 0; i < firstFloorLanes.Count; i++)
            {
                Point p1 = new Point(firstFloorLanes[i], 1);
                //kazda sciezka zaczyna sie w tym samym punkcie startowym (startPoint)
                paths.Add(new List<Point> { startPoint, p1 });
            }

            //generowanie sciezek od pietra 1 do przedostatniego (totalFloors - 2)
            for (int y = 1; y < config.totalFloors - 2; y++)
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    Point current = paths[i].Last();
                    List<int> candidateLanes = new();

                    if (current.x > 0) candidateLanes.Add(current.x - 1);
                    candidateLanes.Add(current.x);
                    if (current.x < config.gridWidth - 1) candidateLanes.Add(current.x + 1);

                    int nextX = candidateLanes[Random.Range(0, candidateLanes.Count)];
                    Point nextPoint = new Point(nextX, y + 1);
                    paths[i].Add(nextPoint);
                }

                //szansa na dodatkowe rozgalezienie
                if (paths.Count < config.gridWidth && Random.value < 0.35f)
                {
                    int idx = Random.Range(0, paths.Count);
                    Point p = paths[idx].Last();
                    List<int> sideLanes = new();
                    if (p.x > 0) sideLanes.Add(p.x - 1);
                    if (p.x < config.gridWidth - 1) sideLanes.Add(p.x + 1);

                    if (sideLanes.Count > 0)
                    {
                        Point branchPoint = new Point(sideLanes[Random.Range(0, sideLanes.Count)], y + 1);
                        List<Point> newBranch = new List<Point>(paths[idx].Take(paths[idx].Count - 1)) { branchPoint };
                        paths.Add(newBranch);
                    }
                }
            }

            //doprowadzenie wszystkich sciezek do Bossa na ostatnim pietrze
            foreach (var path in paths)
            {
                path.Add(bossPoint);
            }

            //zbieranie unikalnych pokojow
            HashSet<Point> uniquePoints = new();
            foreach (var path in paths)
            {
                foreach (var pt in path)
                {
                    uniquePoints.Add(pt);
                }
            }

            //tworzenie obiektow Node i liczenie pozycji
            Dictionary<Point, Node> nodeDict = new();
            foreach (var pt in uniquePoints)
            {
                NodeType type = DetermineType(pt);
                Node node = new Node(type, pt);

                //wyliczanie pozycji w UI
                float xPos = (pt.x - (config.gridWidth - 1) / 2f) * config.xSpacing;
                float yPos = pt.y * config.ySpacing;

                //Start i Boss sa idealnie wycentrowane
                if (pt.y > 0 && pt.y < config.totalFloors - 1)
                {
                    xPos += Random.Range(-config.randomPositionOffset, config.randomPositionOffset);
                    yPos += Random.Range(-config.randomPositionOffset, config.randomPositionOffset);
                }

                node.position = new Vector2(xPos, yPos);
                nodeDict[pt] = node;
                nodes.Add(node);
            }

            //laczenie pokojow (incoming / outgoing)
            foreach (var path in paths)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Node from = nodeDict[path[i]];
                    Node to = nodeDict[path[i + 1]];
                    from.AddOutgoing(to.point);
                    to.AddIncoming(from.point);
                }
            }

            EnforceExactlyThreeEvents(nodes);

            return new Map(nodes, new List<Point>());
        }

        private static NodeType DetermineType(Point pt)
        {
            //ZASADA 1: pietro 0 = start / tutorialowa walka
            if (pt.y == 0) return NodeType.WalkaZwykla;

            //ZASADA 2: ostatnie pietro = Boss
            if (pt.y == config.totalFloors - 1) return NodeType.Boss;

            //ZASADA 3: przedostatnie pietro = zawsze Żerca
            if (pt.y == config.totalFloors - 2) return NodeType.Zerca;

            //posrednie pietra: losowanie wedlug %
            float elitaWeight = pt.y < 4 ? 0f : config.probElita; //brak elitowych przeciwnikow na pietrach 1-3
            float total = config.probWalka + elitaWeight + config.probZerca + config.probWiedzma;
            float roll = Random.Range(0f, total);

            if (roll < config.probWalka) return NodeType.WalkaZwykla;
            roll -= config.probWalka;
            if (roll < elitaWeight) return NodeType.Elita;
            roll -= elitaWeight;
            if (roll < config.probZerca) return NodeType.Zerca;

            return NodeType.Wiedzma;
        }

        private static void EnforceExactlyThreeEvents(List<Node> nodes)
        {
            //wybiera losowo 3 wezly z pieter posrednich (od 2 do N-3) i ustawia je na Event
            var eligibleNodes = nodes.Where(n => n.point.y >= 2 && n.point.y <= config.totalFloors - 3).ToList();
            if (eligibleNodes.Count < 3) return;

            //czysci ewentualne stare eventy
            foreach (var n in nodes.Where(n => n.nodeType == NodeType.Event))
                n.nodeType = NodeType.WalkaZwykla;

            //losuje dokladnie 3 rozne wezly
            for (int i = 0; i < 3; i++)
            {
                int idx = Random.Range(0, eligibleNodes.Count);
                eligibleNodes[idx].nodeType = NodeType.Event;
                eligibleNodes.RemoveAt(idx);
            }
        }

        private static bool ValidateCandidate(Map map)
        {
            if (map == null) return false;

            //sprawdzenie czy sa dokladnie 3 Eventy
            int eventCount = map.nodes.Count(n => n.nodeType == NodeType.Event);
            if (eventCount != 3) return false;

            //sprawdzenie czy istnieja wezly startowe
            var startNodes = map.GetNodesOnFloor(0);
            if (startNodes.Count == 0) return false;

            //weryfikacja kazdej mozliwej sciezki za pomoca DFS
            foreach (var start in startNodes)
            {
                if (!DfsValidatePath(start, map, 0, 0, 0, 0))
                    return false;
            }

            return true;
        }

        private static bool DfsValidatePath(Node current, Map map, int wiedzmy, int zercy, int elity, int eventy)
        {
            if (current.nodeType == NodeType.Wiedzma) wiedzmy++;
            if (current.nodeType == NodeType.Zerca) zercy++;
            if (current.nodeType == NodeType.Elita) elity++;
            if (current.nodeType == NodeType.Event) eventy++;

            //max 2 Wiedźmy (sklepy), max 4 Żerców (ogniska), max 2 Elity, max 2 wejścia do Eventów
            if (wiedzmy > 2 || zercy > 4 || elity > 2 || eventy > 2)
                return false;

            if (current.nodeType == NodeType.Boss)
                return true;

            if (current.outgoing.Count == 0)
                return false; //slepy zaulek

            foreach (var pt in current.outgoing)
            {
                Node next = map.GetNode(pt);
                if (next == null) return false;
                if (!DfsValidatePath(next, map, wiedzmy, zercy, elity, eventy))
                    return false;
            }

            return true;
        }

        private static List<int> GetRandomLanes(int count, int maxLanes)
        {
            List<int> pool = Enumerable.Range(0, maxLanes).ToList();
            List<int> result = new();
            for (int i = 0; i < count && pool.Count > 0; i++)
            {
                int idx = Random.Range(0, pool.Count);
                result.Add(pool[idx]);
                pool.RemoveAt(idx);
            }
            return result;
        }
    }
}
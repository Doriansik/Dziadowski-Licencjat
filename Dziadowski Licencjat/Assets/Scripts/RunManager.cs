using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    public Map.Node CurrentNode { get; private set; }
    public int CurrentFloor { get; private set; } = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EnterNode(Map.Node node)
    {
        CurrentNode = node;
        CurrentFloor = node.point.y;
        Debug.Log($"wejście do pokoju: piętro {node.point.y}, kolumna {node.point.x}, typ: {node.nodeType}");

        //przelaczanie scen/pokoi:
        //switch(node.nodeType) { ... }
    }
    public void ResetRun()
    {
        CurrentNode = null;
        CurrentFloor = -1;
        Debug.Log("mapa została zresetowana.");
    }
}
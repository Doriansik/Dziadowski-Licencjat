namespace Map
{
    public enum NodeType
    {
        WalkaZwykla,   //pietro 0 (tutorial) oraz standardowe walki
        Elita,         //silny wrog
        Event,         //pokoje losowe
        Zerca,         //ognisko / leczenie / ulepszenie (przedostatnie pietro)
        Wiedzma,       //sklep z kartami
        Boss           //ostatnie pietro
    }

    public enum NodeStateType
    {
        Locked,
        Attainable,
        Visited
    }
}
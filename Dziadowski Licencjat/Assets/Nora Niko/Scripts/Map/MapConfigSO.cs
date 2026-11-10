using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Map/MapConfig")]
    public class MapConfigSO : ScriptableObject
    {
        [Header("Rozmiar mapy")]
        [Range(12, 16)] public int totalFloors = 15;
        public int gridWidth = 5; //liczba kolumn (lanes)
        public int numOfPreBossNodes = 2; //liczba Żerców przed bossem
        public int numOfStartingNodes = 2; //liczba ścieżek na dole

        [Header("Odstępy w pikselach (na UI)")]
        public float xSpacing = 160f;
        public float ySpacing = 130f;
        public float randomPositionOffset = 25f;

        [Header("% na spawnowanie danych pokojow")]
        [Range(0f, 1f)] public float probWalka = 0.40f;
        [Range(0f, 1f)] public float probElita = 0.15f;
        [Range(0f, 1f)] public float probEvent = 0.15f;
        [Range(0f, 1f)] public float probZerca = 0.15f;
        [Range(0f, 1f)] public float probWiedzma = 0.15f;
    }
}
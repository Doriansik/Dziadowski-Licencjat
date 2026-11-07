using System.Collections.Generic;
using UnityEngine;

public class StandardDeckShuffler : IDeckShuffler
{
    public void Shuffle(List<CardInstance> deck)
    {
        int deckSize = deck.Count;
        int minimumIndex = 0;

        for (int i = deckSize - 1; i > minimumIndex; i--)
        {
            int randomIndex = Random.Range(minimumIndex, i + 1);
            CardInstance temporaryCard = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temporaryCard;
        }
    }
}
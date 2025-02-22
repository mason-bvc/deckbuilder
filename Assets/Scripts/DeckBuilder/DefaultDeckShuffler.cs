using System;

namespace DeckBuilder
{
    public struct DefaultDeckShuffler : IDeckShuffler
    {
        public readonly void Shuffle(ref Deck deck)
        {
            var rnd = new Random();

            for (int i = 0; i < deck.Cards.Count; i++)
            {
                int indexToSwitchWith = rnd.Next(0, deck.Cards.Count);
                (deck.Cards[i], deck.Cards[indexToSwitchWith]) = (deck.Cards[indexToSwitchWith], deck.Cards[i]);
            }
        }
    }
}

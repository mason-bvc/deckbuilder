using System.Collections.Generic;

namespace DeckBuilder
{
    public struct Game
    {
        public Player PlayerOne;
        public Player PlayerTwo;
        public IDeckShuffler DeckShuffler;

        public void Initialize()
        {
            DeckShuffler = new DefaultDeckShuffler();
            InitializePlayer(ref PlayerOne);
            InitializePlayer(ref PlayerTwo);
        }

        public void Begin()
        {
            DeckShuffler.Shuffle(ref PlayerOne.Deck);
            DeckShuffler.Shuffle(ref PlayerTwo.Deck);
            PlayerOne.DrawFromDeckIntoHand(5);
            PlayerTwo.DrawFromDeckIntoHand(5);
        }

        private readonly void InitializePlayer(ref Player player)
        {
            player.Deck.Initialize(new List<CardType>());
            player.Hand.Initialize(new List<HeldCard>());
            Deck.PushFullDeckInto(player.Deck.Cards);
        }
    }
}

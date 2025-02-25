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

            // for (int i = 0; i < 5; i++)
            // {
            //     PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.Ace, House.Clubs)));
            // }

            PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.King, House.Diamonds)));
            PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.King, House.Clubs)));
            PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.King, House.Hearts)));
            PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.Jack, House.Spades)));
            PlayerOne.Hand.Cards.Add(new HeldCard(new CardType(Rank.Jack, House.Hearts)));

            // PlayerOne.DrawFromDeckIntoHand(5);
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

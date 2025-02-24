using System.Collections.Generic;
using System.Linq;

namespace DeckBuilder
{
    public struct Player
    {
        public Deck Deck;
        public Hand Hand;

        public void Initialize(Deck deck, Hand hand)
        {
            Deck = deck;
            Hand = hand;
        }

        public void DrawFromDeckIntoHand(int count)
        {
            List<CardType> drawnCards = new();
            List<HeldCard> nowHeldCards;

            Deck.Draw(count, drawnCards);
            nowHeldCards = drawnCards.Select(cardType => new HeldCard(cardType)).ToList();
            Hand.Cards = Hand.Cards.Concat(nowHeldCards).ToList();
        }
    }
}

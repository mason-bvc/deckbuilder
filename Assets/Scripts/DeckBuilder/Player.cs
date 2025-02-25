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

        public readonly bool FindHeldCardIndexByID(int id, out int index)
        {
            index = -1;

            for (int i = 0; i < Hand.Cards.Count; i++)
            {
                if (Hand.Cards[i].ID == id)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public void DrawFromDeckIntoHand(int count)
        {
            List<CardType> drawnCards = new();
            List<HeldCard> nowHeldCards;

            Deck.Draw(count, drawnCards);
            nowHeldCards = drawnCards.Select(cardType => new HeldCard(cardType)).ToList();
            Hand.Cards = Hand.Cards.Concat(nowHeldCards).ToList();
        }

        public void DiscardSelected()
        {
            foreach (var card in Hand.Cards)
            {
                if (card.IsSelected)
                {
                }
            }
        }
    }
}

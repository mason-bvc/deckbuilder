using System.Collections.Generic;

namespace DeckBuilder
{
    public struct Hand
    {
        public IList<HeldCard> Cards;

        public void Initialize(IList<HeldCard> freshList)
        {
            Cards = freshList;
        }

        public readonly void GetHeldCardTypes(ICollection<CardType> cardTypes)
        {
            foreach (var heldCard in Cards)
            {
                cardTypes.Add(heldCard.CardType);
            }
        }
    }
}

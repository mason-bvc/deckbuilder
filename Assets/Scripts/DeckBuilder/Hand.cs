using System.Collections.Generic;

namespace DeckBuilder
{
    public struct Hand
    {
        public IList<CardType> Cards;

        public void Initialize(IList<CardType> cardList)
        {
            Cards = cardList;
        }
    }
}

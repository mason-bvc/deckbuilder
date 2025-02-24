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
    }
}

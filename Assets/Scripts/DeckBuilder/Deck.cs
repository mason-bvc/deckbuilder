using System.Collections.Generic;

namespace DeckBuilder
{
    public struct Deck
    {
        public IList<CardType> Cards;

        public static void PushFullDeckInto(ICollection<CardType> cardCollection)
        {
            // can't do LINQ here due to static type declaration reasons
            foreach (CardType cardType in CardType.All)
            {
                cardCollection.Add(cardType);
            }
        }

        // Not using a constructor because I may or may not want lazy
        // initialization. I also don't want to have to strictly enforce boxing
        // and unnecessarily obfuscate the API by wrapping in Lazy<T>, even if
        // it may be safer that way.
        public void Initialize(IList<CardType> cardList)
        {
            Cards = cardList;
        }

        public readonly void Draw(int count, ICollection<CardType> into)
        {
            for (int i = 0; i < count; i++)
            {
                // man, i wish IList<T> had a pop method.
                into.Add(Cards[^1]);
                Cards.RemoveAt(Cards.Count - 1);
            }
        }
    }
}

using System;

namespace DeckBuilder
{
    public struct HeldCard : IEquatable<CardType>
    {
        public CardType CardType;
        public bool IsSelected;

        //
        // TODO: GROSS KLUDGE
        //
        private static int _nextID;
        public int ID { get; private set; }

        public static explicit operator CardType(HeldCard heldCard) => heldCard.CardType;

        public HeldCard(CardType cardType)
        {
            ID = _nextID++;
            CardType = cardType;
            IsSelected = false;
        }

        public readonly bool Equals(CardType other) => CardType.Equals(other);
    }
}

using System;

namespace DeckBuilder
{
    public struct HeldCard : IEquatable<CardType>
    {
        public CardType CardType;
        public bool IsSelected;

        public static explicit operator CardType(HeldCard heldCard) => heldCard.CardType;
        public static explicit operator HeldCard(CardType cardType) => new(cardType);

        public HeldCard(CardType cardType)
        {
            CardType = cardType;
            IsSelected = false;
        }

        public readonly bool Equals(CardType other) => CardType.Equals(other);
    }
}

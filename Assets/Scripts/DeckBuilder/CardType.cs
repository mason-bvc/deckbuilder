using System;
using System.Collections.Generic;

namespace DeckBuilder
{
    public struct CardType : IEquatable<CardType>
    {
        // TODO: cache this statically
        public static IEnumerable<CardType> All
        {
            get
            {
                foreach (House house in Enum.GetValues(typeof(House)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        yield return new CardType(rank, house);
                    }
                }
            }
        }

        public Rank Rank;
        public House House;

        public static implicit operator CardType(ValueTuple<Rank, House> tuple) => new(tuple.Item1, tuple.Item2);
        public static implicit operator CardType(ValueTuple<House, Rank> tuple) => new(tuple.Item2, tuple.Item1);
        public static implicit operator ValueTuple<Rank, House>(CardType cardType) => (cardType.Rank, cardType.House);
        public static implicit operator ValueTuple<House, Rank>(CardType cardType) => (cardType.House, cardType.Rank);

        public CardType(Rank rank, House house)
        {
            Rank = rank;
            House = house;
        }

        public override readonly int GetHashCode() => Rank.GetHashCode() ^ House.GetHashCode();

        //
        // IEquatable<CardType>
        //
        public readonly bool Equals(CardType other) => Rank == other.Rank && House == other.House;
    }
}

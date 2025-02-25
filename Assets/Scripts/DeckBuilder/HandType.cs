using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace DeckBuilder
{
    public enum HandType
    {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush,
    }

    public enum StraightType
    {
        None,
        Normal,
        Royal,
    }

    //
    // ok now i'm just showing off
    //
    public static class HandTypeUtilities
    {
        public static int GetHashForCardTypeQualities<T>(IEnumerable<CardType> cards, Func<CardType, T> enumValueGetter)
            where T : Enum
            => cards.Aggregate(0, (accum, cardType) => accum | (1 << Convert.ToInt32(enumValueGetter(cardType))));

        public static bool AreAllSame<T>(IEnumerable<CardType> cards, Func<CardType, T> enumValueGetter, ref T quality) where T : Enum
        {
            int hash = GetHashForCardTypeQualities(cards, enumValueGetter);
            bool areAllSame = false;

            foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                areAllSame |= hash == 1 << Convert.ToInt32(enumValue);

                if (areAllSame)
                {
                    quality = enumValue;
                }
            }

            return areAllSame;
        }

        public static StraightType CalculateStraightType(ICollection<CardType> cards)
        {
            int royalFlushHash = (1 << (int)Rank.Ace) | (1 << (int)Rank.King) | (1 << (int)Rank.Queen) | (1 << (int)Rank.Jack) | (1 << (int)Rank.Ten);
            // ugh allocations
            List<int> sortedCardRanks = cards.Select(cardType => (int)cardType.Rank + 1).ToList();
            StraightType straightType = StraightType.None;
            bool isStraight = false;

            sortedCardRanks.Sort();

            int begin = 0;
            int end = 5;
            int hash = GetHashForCardTypeQualities(cards, cardType => cardType.Rank);

            if (hash == royalFlushHash)
            {
                return StraightType.Royal;
            }

            // sliding window: 0b00[111]
            //                 0b0[011]1
            //                 0b[001]11
            //                 etc.
            for (; end <= Enum.GetValues(typeof(Rank)).Length; begin++, end++)
            {
                int desiredHash = 0;

                for (int i = begin; i < end; i++)
                {
                    desiredHash |= 1 << i;
                }

                isStraight |= hash == desiredHash;
            }

            if (isStraight)
            {
                straightType = StraightType.Normal;
            }

            return straightType;
        }

        public static Dictionary<Rank, int> GenerateRankMap(IEnumerable<CardType> cards)
        {
            // TODO: is this really dumb or secretly the best
            // also maybe I should consider doing it this way to calculate
            // the flushes too?

            Rank[] rankValues = (Rank[])Enum.GetValues(typeof(Rank));
            Dictionary<Rank, int> countMap = new(rankValues.Length);

            foreach (var cardType in cards)
            {
                if (!countMap.ContainsKey(cardType.Rank))
                {
                    countMap[cardType.Rank] = 0;
                }

                countMap[cardType.Rank] += 1;
            }

            return countMap;
        }

        public static HandType Ascertain(ICollection<CardType> cards)
        {
            House house = default;
            bool areAllSameHouse = AreAllSame(cards, cardType => cardType.House, ref house);
            StraightType straightType = CalculateStraightType(cards);

            if (areAllSameHouse)
            {
                if (straightType == StraightType.Royal)
                {
                    return HandType.RoyalFlush;
                }

                if (straightType == StraightType.Normal)
                {
                    return HandType.StraightFlush;
                }

                return HandType.Flush;
            }

            if (straightType == StraightType.Normal)
            {
                return HandType.Straight;
            }

            var rankMap = GenerateRankMap(cards);

            //
            // TODO: oh god this is stupid
            //
            foreach (var kvp in rankMap)
            {
                if (rankMap[kvp.Key] >= 4)
                {
                    return HandType.FourOfAKind;
                }

                if (rankMap[kvp.Key] >= 3)
                {
                    rankMap.Remove(kvp.Key);

                    foreach (var kvpp in rankMap)
                    {
                        if (rankMap[kvpp.Key] >= 2)
                        {
                            return HandType.FullHouse;
                        }
                    }

                    return HandType.ThreeOfAKind;
                }

                if (rankMap[kvp.Key] >= 2)
                {
                    return HandType.Pair;
                }
            }

            return HandType.HighCard;
        }
    }
}

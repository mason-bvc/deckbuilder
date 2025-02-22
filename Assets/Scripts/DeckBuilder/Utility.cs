using System.Collections.Generic;
using System.Text;

#nullable enable

namespace DeckBuilder
{
    public static class Utility
    {
        private static readonly Dictionary<Rank, string> _friendlyRankNames = new();
        private static readonly Dictionary<House, string> _friendlyHouseNames = new();
        private static readonly Dictionary<CardType, string> _friendlyCardNames = new();

        public static string GetFriendlyName(Rank rank) => _friendlyRankNames[rank];
        public static string GetFriendlyName(House house) => _friendlyHouseNames[house];
        public static string GetFriendlyName(CardType cardType) => _friendlyCardNames[cardType];

        static Utility()
        {
            CacheFriendlyNames();
        }

        private static string? MaybeGetRankName(Rank rank) => rank switch
        {
            Rank.Two => "2",
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "10",
            Rank.Jack => "Jack",
            Rank.Queen => "Queen",
            Rank.King => "King",
            Rank.Ace => "Ace",
            _ => null,
        };

        private static string? MaybeGetHouseName(House house) => house switch
        {
            House.Diamonds => "Diamonds",
            House.Clubs => "Clubs",
            House.Hearts => "Hearts",
            House.Spades => "Spades",
            _ => null,
        };

        private static string GetRankName(Rank rank)
            => MaybeGetRankName(rank) ?? "<INVALID RANK NAME>";

        private static string GetHouseName(House house)
            => MaybeGetHouseName(house) ?? "<INVALID HOUSE NAME>";

        private static string GenerateFriendlyCardTypeName(CardType cardType)
        {
            var sb = new StringBuilder();

            sb.Append(GetRankName(cardType.Rank));
            sb.Append(" of ");
            sb.Append(GetHouseName(cardType.House));

            return sb.ToString();
        }

        private static void CacheFriendlyNames()
        {
            foreach (CardType cardType in CardType.All)
            {
                _friendlyCardNames[cardType] = GenerateFriendlyCardTypeName(cardType);
            }
        }
    }
}

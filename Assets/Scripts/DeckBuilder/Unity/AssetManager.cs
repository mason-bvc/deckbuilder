using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable enable

namespace DeckBuilder.Unity
{
    public static class AssetManager
    {
        public enum AudioClipName
        {
            Card,
            BackgroundMusic,
        }

        public enum PrefabName
        {
            Card,
        }

        public enum SpriteName
        {
            CardBackside,
        }

        public class LoadSpriteException : Exception
        {
            public LoadSpriteException() { }
            public LoadSpriteException(string message) : base(message) { }
        }

        private const string PREFABS_BASE_PATH = "Prefabs/";
        private const string CARD_SPRITE_BASE_PATH = "Textures/Cards/";

        public static readonly AudioClip[] AudioClips = new AudioClip[Enum.GetValues(typeof(AudioClipName)).Length];
        public static readonly GameObject[] Prefabs = new GameObject[Enum.GetValues(typeof(PrefabName)).Length];
        public static readonly Sprite[] Sprites = new Sprite[Enum.GetValues(typeof(SpriteName)).Length];
        public static IReadOnlyDictionary<CardType, Sprite> CardSprites => _cardSprites;

        private static readonly Dictionary<CardType, Sprite> _cardSprites = new();
        private static readonly StringBuilder _assetPathStringBuilder = new();
        private static readonly StringBuilder _loadAllCardSpritesStringBuilder = new();

        public static void LoadAndCacheAllAssets()
        {
            Sprites[(int)SpriteName.CardBackside] = Resources.Load<Sprite>(CARD_SPRITE_BASE_PATH + "card_back_red");
            Prefabs[(int)PrefabName.Card] = Resources.Load<GameObject>(PREFABS_BASE_PATH + "Card");
            LoadAndCacheAllCardSprites();
        }

        private static string? GetAssetInfix(Rank rank) => rank switch
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
            Rank.Jack => "jack",
            Rank.Queen => "queen",
            Rank.King => "king",
            Rank.Ace => "ace",
            _ => null,
        };

        private static string? GetAssetInfix(House house) => house switch
        {
            House.Diamonds => "diamonds",
            House.Clubs => "clubs",
            House.Hearts => "hearts",
            House.Spades => "spades",
            _ => null,
        };

        private static string? GenerateAssetFileName(CardType cardType)
        {
            string? rankInfix = GetAssetInfix(cardType.Rank);
            string? houseInfix = GetAssetInfix(cardType.House);

            if (rankInfix is null || houseInfix is null)
            {
                return null;
            }

            _assetPathStringBuilder.Clear();
            _assetPathStringBuilder.Append(rankInfix);
            _assetPathStringBuilder.Append("_of_");
            _assetPathStringBuilder.Append(houseInfix);

            return _assetPathStringBuilder.ToString();
        }

        private static Sprite LoadCardSprite(CardType cardType)
        {
            string? assetFileName = GenerateAssetFileName(cardType)
                ?? throw new LoadSpriteException(
                    $"Could not generate filename for \"{Utility.GetFriendlyName(cardType)}\"");

            _loadAllCardSpritesStringBuilder.Clear();
            _loadAllCardSpritesStringBuilder.Append(CARD_SPRITE_BASE_PATH);
            _loadAllCardSpritesStringBuilder.Append(assetFileName);

            string assetPath = _loadAllCardSpritesStringBuilder.ToString();

            return Resources.Load<Sprite>(assetPath);
        }

        private static void LoadAndCacheAllCardSprites()
        {
            foreach (House house in Enum.GetValues(typeof(House)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    var cardType = new CardType(rank, house);
                    Sprite? cardSprite = LoadCardSprite(cardType);

                    if (cardSprite is not null)
                    {
                        _cardSprites[cardType] = cardSprite;
                    }
                }
            }
        }
    }
}

using UnityEngine;

namespace DeckBuilder.Unity
{
    public class Game : MonoBehaviour
    {
        private DeckBuilder.Game _game;
        private Transform _playerOneHandOrigin;
        private Transform _playerTwoHandOrigin;

        #region Unity Messages

        public void Awake()
        {
            AssetManager.LoadAndCacheAllAssets();
            _game.Initialize();
        }

        public void Start()
        {
            _playerOneHandOrigin = transform.Find("PlayerOneHandOrigin");
            _playerTwoHandOrigin = transform.Find("PlayerTwoHandOrigin");
            _game.Begin();

            int cardCount = _game.PlayerOne.Hand.Cards.Count;

            for (int i = 0; i < cardCount; i++)
            {
                CardType cardType = _game.PlayerOne.Hand.Cards[i];
                GameObject cardInstance = Instantiate(AssetManager.CardPrefab, _playerOneHandOrigin);
                Card cardComponent = cardInstance.GetComponent<Card>();
                SpriteRenderer cardSpriteRenderer = cardComponent.Leaf.GetComponent<SpriteRenderer>();

                cardSpriteRenderer.sprite = AssetManager.CardSprites[cardType];
                cardInstance.transform.Translate(Vector3.right * Mathf.Lerp(-5, 5, i / (float)(cardCount - 1)), Space.World);
            }
        }

        #endregion Unity Messages
    }
}

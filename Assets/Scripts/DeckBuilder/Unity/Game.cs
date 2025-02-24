using System.Linq;
using Belmondo;
using TMPro;
using UnityEngine;

namespace DeckBuilder.Unity
{
    public class Game : MonoBehaviour
    {
        /// <summary>
        /// How much room the player's hand takes up in units.
        /// </summary>
        private const float HALF_HAND_SPAN = 3;

        private DeckBuilder.Game _game;
        private Transform _playerOneHandOrigin;
        private Transform _playerTwoHandOrigin;
        private TMP_Text _phaseText;

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
            _phaseText = transform.Find("Canvas/PhaseText").GetComponent<TMP_Text>();
            SetUpBoard();
        }

        #endregion Unity Messages

        public void SetUpBoard()
        {
            _game.Begin();

            int cardCount = _game.PlayerOne.Hand.Cards.Count;

            for (int i = 0; i < cardCount; i++)
            {
                HeldCard heldCard = _game.PlayerOne.Hand.Cards[i];
                GameObject cardInstance = Instantiate(AssetManager.Prefabs[(int)AssetManager.PrefabName.Card], _playerOneHandOrigin);
                Card cardComponent = cardInstance.GetComponent<Card>();
                SpriteRenderer cardSpriteRenderer = cardComponent.Leaf.GetComponent<SpriteRenderer>();

                cardComponent.Selected.AddListener(OnCardSelected);
                cardSpriteRenderer.sprite = AssetManager.CardSprites[heldCard.CardType];
                cardInstance.transform.Translate(Vector3.right * Mathf.Lerp(-HALF_HAND_SPAN, HALF_HAND_SPAN, i / (float)(cardCount - 1)), Space.World);
            }
        }

        private void OnCardSelected(Card card)
        {
            card.HeldCard.IsSelected = true;
            // if Hand.Cards is a list and not an OrderedDictionary,
            // this incurrs a O(n) cost but because n is likely 5 it shouldn't
            // matter.
            _game.PlayerOne.Hand.Cards.Contains((HeldCard)card.HeldCard.CardType);
        }
    }
}

using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private HashSet<Card> _selectedCards = new();

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

        //
        // TODO: use events instead
        //
        public void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                PlayHand();
            }

            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                Discard();
            }
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

                cardComponent.HeldCardID = heldCard.ID;
                cardComponent.Selected.AddListener(OnCardSelected);
                cardComponent.Deselected.AddListener(OnCardDeselected);
                cardSpriteRenderer.sprite = AssetManager.CardSprites[heldCard.CardType];
                cardInstance.transform.Translate(Vector3.right * Mathf.Lerp(-HALF_HAND_SPAN, HALF_HAND_SPAN, i / (float)(cardCount - 1)), Space.World);
            }
        }

        private void PlayHand()
        {
            // TODO: cache this
            List<CardType> slicedHeldCards = new(_game.PlayerOne.Hand.Cards.Count);

            _game.PlayerOne.Hand.GetHeldCardTypes(slicedHeldCards);
            Debug.Log(HandTypeUtilities.Ascertain(slicedHeldCards));
        }

        private void Discard()
        {
            HashSet<Card> copy = new(_selectedCards);

            foreach (var card in _selectedCards)
            {
                Destroy(card.gameObject);
            }

            foreach (var card in copy)
            {
                _selectedCards.Remove(card);
            }

            _game.PlayerOne.DrawFromDeckIntoHand(copy.Count);
        }

        private void OnCardSelected(Card card)
        {
            if (_game.PlayerOne.FindHeldCardIndexByID(card.HeldCardID, out int heldCardIndex))
            {
                var heldCards = _game.PlayerOne.Hand.Cards;
                var newHeldCard = heldCards[heldCardIndex];

                newHeldCard.IsSelected = true;
                heldCards[heldCardIndex] = newHeldCard;
            }

            _selectedCards.Add(card);
        }

        private void OnCardDeselected(Card card)
        {
            if (_game.PlayerOne.FindHeldCardIndexByID(card.HeldCardID, out int heldCardIndex))
            {
                var heldCards = _game.PlayerOne.Hand.Cards;
                var newHeldCard = heldCards[heldCardIndex];

                newHeldCard.IsSelected = false;
                heldCards[heldCardIndex] = newHeldCard;
            }

            _selectedCards.Remove(card);
        }
    }
}

using System;
using Belmondo;
using UnityEngine;
using UnityEngine.Events;

namespace DeckBuilder.Unity
{
    public enum CardState
    {
        Idle,
        Selected,
    }

    public class Card : MonoBehaviour
    {
        public readonly UnityEvent<Card> Selected = new();
        public readonly UnityEvent<Card> Deselected = new();

        [HideInInspector]
        public AudioSource AudioSource;
        [HideInInspector]
        public EventBroadcaster EventBroadcaster;
        [HideInInspector]
        public Transform Leaf;
        [HideInInspector]
        public Transform SelectTransform;
        [HideInInspector]
        public Tween<float> SelectTransformTween = new();
        [HideInInspector]
        public int HeldCardID;
        [HideInInspector]
        public CardState State;

        #region Unity Messages

        public void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            SelectTransform = transform.Find("SelectTransform");
            SelectTransformTween
                .SetLerpDelegate(EasingFunction.EaseInOutCubic)
                .SetSetterDelegate(y => SelectTransform.localPosition = Vector2.up * y);
            SelectTransformTween
                .SetLerpDelegate(EasingFunction.EaseInOutCubic)
                .SetSetterDelegate(y => SelectTransform.localPosition = Vector2.up * y);
            Leaf = SelectTransform.Find("Leaf");
            EventBroadcaster = Leaf.GetComponent<EventBroadcaster>();
            EventBroadcaster.MouseDown.AddListener(OnMouseDownOnLeaf);
        }

        public void Update()
        {
            SelectTransformTween.Update(TimeSpan.FromSeconds(Time.deltaTime));
        }

        #endregion Unity Messages

        public async void SetState(CardState state)
        {
            if (SelectTransformTween.Status == TweenStatus.Running)
            {
                return;
            }

            if (State == CardState.Idle)
            {
                Selected.Invoke(this);

                await SelectTransformTween
                    .Run(0, 1, TimeSpan.FromSeconds(0.25));
            }
            else if (State == CardState.Selected)
            {
                Deselected.Invoke(this);

                await SelectTransformTween
                    .Run(SelectTransform.localPosition.y, 0, TimeSpan.FromSeconds(0.25));
            }

            State = state;
        }

        private void OnMouseDownOnLeaf()
        {
            SetState(State switch
            {
                CardState.Idle => CardState.Selected,
                CardState.Selected => CardState.Idle,
                _ => CardState.Idle,
            });
        }
    }
}

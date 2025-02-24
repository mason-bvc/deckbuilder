using System;
using System.Threading.Tasks;
using Belmondo;
using UnityEngine;

namespace DeckBuilder.Unity
{
    public enum CardState
    {
        Idle,
        Selected,
    }

    public class Card : MonoBehaviour
    {
        [HideInInspector]
        public AudioSource AudioSource;
        [HideInInspector]
        public EventBroadcaster EventBroadcaster;
        [HideInInspector]
        public Transform Leaf;
        [HideInInspector]
        public Transform SelectTransform;
        [HideInInspector]
        public Tween<float> SelectTransformTween;
        [HideInInspector]
        public CardState State;

        #region Unity Messages

        public void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            SelectTransform = transform.Find("SelectTransform");
            Leaf = SelectTransform.Find("Leaf");
            EventBroadcaster = Leaf.GetComponent<EventBroadcaster>();
            EventBroadcaster.MouseDown.AddListener(OnMouseDownOnLeaf);
        }

        #endregion Unity Messages

        public async void SetState(CardState state)
        {
            if (State == CardState.Idle)
            {
            }
            else if (State == CardState.Selected)
            {
                // await Tweener
                //     .SetLerpFunction(EasingFunction.EaseOutCubic)
                //     .Begin(SelectTransform.localPosition.y, 0, 0.25F)
                //     .Finished();
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

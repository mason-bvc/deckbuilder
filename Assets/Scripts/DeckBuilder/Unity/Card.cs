using UnityEngine;
using System.Threading.Tasks;
using Belmondo;

namespace DeckBuilder.Unity
{
    public enum CardState
    {
        Idle,
        Selected,
    }

    public class Card : MonoBehaviour
    {
        public AudioSource AudioSource;
        public EventBroadcaster EventBroadcaster;
        public Transform Leaf;
        public Transform SelectTransform;
        public Tween<Vector2> SelectTransformTweener;
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
                await SelectTransformTweener.Run(Vector2.zero, Vector2.up, 0.25F);

                // await Tweener
                //     .SetLerpFunction((out float value, float from, float to, float t) =>
                //     {
                //         value = EasingFunction.EaseInOutCubic(from, to, t);
                //     })
                //     .Begin(SelectTransform.localPosition.y, 1, 0.25F)
                //     .Finished();
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
            SetState(State switch {
                CardState.Idle => CardState.Selected,
                CardState.Selected => CardState.Idle,
                _ => CardState.Idle,
            });
        }
    }
}

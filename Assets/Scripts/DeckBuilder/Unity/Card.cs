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
        public AudioSource AudioSource;
        public EventBroadcaster EventBroadcaster;
        public Transform Leaf;
        public Lerper Lerper;
        public Transform SelectTransform;
        public CardState State;

        #region Unity Messages

        public void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            SelectTransform = transform.Find("SelectTransform");
            Leaf = SelectTransform.Find("Leaf");
            EventBroadcaster = Leaf.GetComponent<EventBroadcaster>();
            EventBroadcaster.MouseDown.AddListener(OnMouseDownOnLeaf);
            Lerper = GetComponent<Lerper>();
        }

        #endregion Unity Messages

        public async void SetState(CardState state)
        {
            if (State == CardState.Idle)
            {
                await Lerper
                    .SetGetterFunction(() => SelectTransform.localPosition.y)
                    .SetSetterFunction(f => SelectTransform.localPosition = Vector2.up * f)
                    .SetLerpFunction(EasingFunction.EaseOutCubic)
                    .Begin(SelectTransform.localPosition.y, 1, 0.25F)
                    .Finished();
            }
            else if (State == CardState.Selected)
            {
                await Lerper
                    .SetGetterFunction(() => SelectTransform.localPosition.y)
                    .SetSetterFunction(f => SelectTransform.localPosition = Vector2.up * f)
                    .SetLerpFunction(EasingFunction.EaseOutCubic)
                    .Begin(SelectTransform.localPosition.y, 0, 0.25F)
                    .Finished();
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

using UnityEngine;

namespace DeckBuilder.Unity
{
    public class Card : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Selected,
        }

        private State _state;
        private Lerper _lerper;
        public Transform SelectTransform;
        public Transform Leaf;
        public EventBroadcaster EventBroadcaster;

        #region Unity Messages

        public void Awake()
        {
            _lerper = GetComponent<Lerper>();
            SelectTransform = transform.Find("SelectTransform");
            Leaf = SelectTransform.Find("Leaf");
            EventBroadcaster = Leaf.GetComponent<EventBroadcaster>();
            EventBroadcaster.MouseDown.AddListener(OnMouseDownOnLeaf);
        }

        #endregion Unity Messages

        public async void SetState(State state)
        {
            _state = state;

            await _lerper
                .SetGetterFunction(() => SelectTransform.localPosition.y)
                .SetSetterFunction(f => SelectTransform.localPosition = Vector2.up * f)
                .SetLerpFunction(EasingFunction.EaseOutCubic)
                .Begin(SelectTransform.localPosition.y, 1, 0.25F)
                .Finished();
        }

        private void OnMouseDownOnLeaf()
        {
            SetState(State.Selected);
        }
    }
}

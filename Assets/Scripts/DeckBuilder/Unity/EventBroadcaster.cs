using UnityEngine;
using UnityEngine.Events;

namespace DeckBuilder.Unity
{
    public class EventBroadcaster : MonoBehaviour
    {
        public UnityEvent MouseDown;

        public void OnMouseDown()
        {
            MouseDown.Invoke();
        }
    }
}

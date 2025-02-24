using System.Collections.Generic;
using Belmondo;
using UnityEngine;

namespace DeckBuilder.Unity
{
    public class TweenerComponent : MonoBehaviour
    {
        private List<Tween<dynamic>> _tweeners;
        private List<Tween<dynamic>> _fixedTweeners;

        public void Update()
        {
            foreach (var tweener in _tweeners)
            {
                tweener.Update();
            }
        }
    }
}

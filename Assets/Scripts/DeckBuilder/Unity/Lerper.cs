using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DeckBuilder.Unity
{
    public class Lerper : MonoBehaviour
    {
        public enum State
        {
            Uninitialized,
            Initialized,
            Running,
            Finished,
        }

        public Func<float, float, float, float> LerpFunction = Mathf.Lerp;
        public Func<float> GetterFunction;
        public Action<float> SetterFunction;
        public float From;
        public float To;
        public State CurrentState;
        public float CurrentT;
        public float CurrentValue;
        public float DurationSeconds;

        #region Unity Messages

        public void Update()
        {
            if (CurrentState == State.Running)
            {
                CurrentT += Time.deltaTime / DurationSeconds;
                SetterFunction(LerpFunction(From, To, CurrentT));

                if (CurrentT >= 1.0F)
                {
                    CurrentState = State.Finished;
                }
            }
        }

        #endregion Uniy Messages

        public Lerper SetGetterFunction(Func<float> getter)
        {
            GetterFunction = getter;
            return this;
        }

        public Lerper SetSetterFunction(Action<float> setter)
        {
            SetterFunction = setter;
            return this;
        }

        public Lerper SetLerpFunction(Func<float, float, float, float> lerpFunction)
        {
            LerpFunction = lerpFunction;
            return this;
        }

        public Lerper Begin(float from, float to, float durationSeconds)
        {
            From = from;
            To = to;
            DurationSeconds = durationSeconds;
            CurrentState = State.Running;

            return this;
        }

        public async Task Finished()
        {
            await Task.Run(() =>
            {
                while (CurrentState != State.Finished) ;
            });
        }
    }
}

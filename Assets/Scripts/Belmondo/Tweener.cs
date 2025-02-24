#nullable enable

using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Belmondo
{
    public enum TweenStatus
    {
        Stopped,
        Running,
        Finished,
    }

    public struct Tween<T>
    {
        private Func<T, T, float, T> _tweenDelegate;
        private T _from;
        private T _to;
        private float _currentT;
        private float _duration;
        private TweenStatus _status;

        public Task Run(T from, T to, float duration)
        {
            _from = from;
            _to = to;
            _duration = duration;
            _status = TweenStatus.Running;

            var self = this;

            return new Task(() =>
            {
                while (self._status != TweenStatus.Finished) ;
            });
        }

        public void Update(ref T value, Func<T, T, float, T> lerpFunction, float deltaTime)
        {
            if (_status != TweenStatus.Running || _currentT >= 1.0F)
            {
                return;
            }

            _currentT += deltaTime / _duration;
            _currentT = Mathf.Clamp(_currentT, 0, 1);
            value = lerpFunction(_from, _to, _currentT);
        }
    }
}

/*
using System.Runtime.CompilerServices;

namespace Belmondo
{
    public enum TweenerState
    {
        NotYetStarted,
        Running,
        Finished,
    }

    public struct TweenerState<T>
    {
        public T From;
        public T To;
        public TweenerState CurrentState;
        public float CurrentT;
        public float DurationSeconds;
    }

    public class TweenerBuilder<T>
    {
        public TweenerState<T> State;

        public IValueTweener<T> CreateValueTweener()
        {
        }

        public void Update(ref T value, float deltaTime)
        {
            if (State.CurrentState == TweenerState.Running)
            {
                State.CurrentT += deltaTime / State.DurationSeconds;
                Lerp(out value, State.From, State.To, State.CurrentT);

                if (State.CurrentT >= 1.0F)
                {
                    State.CurrentState = TweenerState.Finished;
                }
            }
        }

        public ITweenerAPI<T> SetLerpFunction(ITweenerAPI<T>.LerpFunction lerpFunction)
        {
            LerpFunction = lerpFunction;
            return this;
        }

        public ITweenerAPI<T> Begin(T from, T to, float durationSeconds)
        {
            State = new TweenerState<T>
            {
                From = from,
                To = to,
                DurationSeconds = durationSeconds,
                CurrentState = TweenerState.Running,
                CurrentT = 0,
            };

            return this;
        }
    }
}
*/

#nullable restore

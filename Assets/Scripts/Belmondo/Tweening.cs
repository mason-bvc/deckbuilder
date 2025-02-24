#nullable enable

using System;
using System.Threading.Tasks;

namespace Belmondo
{
    public enum TweenStatus
    {
        Stopped,
        Running,
        Finished,
    }

    public class Tween<T>
    {
        private float _t;
        public T From { get; private set; }
        public T To { get; private set; }
        public TimeSpan Duration { get; private set; }
        public TweenStatus Status { get; private set; }

        public Tween(T from, T to, TimeSpan duration)
        {
            From = from;
            To = to;
            Duration = duration;
        }

        public void Update(TimeSpan deltaTime)
        {
            if (Status != TweenStatus.Running)
            {
                return;
            }

            _t += (float)(deltaTime / Duration);
            _t = Math.Clamp(_t, 0.0F, 1.0F);

            if (_t >= 1.0F)
            {
                Status = TweenStatus.Finished;
            }
        }

        public Task Run()
        {
            return new Task(() =>
            {
                while (Status != TweenStatus.Finished) ;
            });
        }
    }
}

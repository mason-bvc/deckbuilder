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

    public class Tween<T> where T : new()
    {
        public delegate T LerpDelegate(T from, T to, float t);

        public float CurrentT { get; private set; }
        public LerpDelegate? Lerp { get; private set; }
        public Action<T>? Setter { get; private set; }
        public T From { get; private set; } = new();
        public T To { get; private set; } = new();
        public TimeSpan Duration { get; private set; }
        public TweenStatus Status { get; private set; }
        public Task? Task { get; private set; }

        public void Update(TimeSpan deltaTime)
        {
            bool shouldUpdate = true;

            shouldUpdate = shouldUpdate && Status == TweenStatus.Running;
            shouldUpdate = shouldUpdate && Setter is not null;
            shouldUpdate = shouldUpdate && Lerp is not null;

            // blagh
            if (!shouldUpdate)
            {
                return;
            }

            CurrentT += (float)(deltaTime / Duration);
            CurrentT = Math.Clamp(CurrentT, 0.0F, 1.0F);
            Setter!.Invoke(Lerp!.Invoke(From, To, CurrentT));

            if (CurrentT == 1.0F)
            {
                Status = TweenStatus.Finished;
            }
        }

        public Tween<T> SetLerpDelegate(LerpDelegate @delegate)
        {
            Lerp = @delegate;
            return this;
        }

        public Tween<T> SetSetterDelegate(Action<T> @delegate)
        {
            Setter = @delegate;
            return this;
        }

        public Task Run(T from, T to, TimeSpan duration)
        {
            CurrentT = 0;
            From = from;
            To = to;
            Duration = duration;
            Status = TweenStatus.Running;

            Task = Task.Run(() =>
            {
                while (Status != TweenStatus.Finished) ;
            });

            return Task;
        }
    }
}

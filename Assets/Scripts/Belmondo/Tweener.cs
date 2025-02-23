using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Belmondo
{
    public delegate T TweenerLerpFunction<T>(out T value, T from, T to, double t);

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
        public double CurrentT;
        public double DurationSeconds;
    }

    public class TweenerBuilder<T> : ITweener<T>
    {
        public TweenerState<T> State;
        public ITweener<T>.LerpFunction LerpFunction;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Lerp(out T value, T from, T to, double t) => LerpFunction(out value, from, to, t);

        public void Update(ref T value, double deltaTime)
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

        public ITweener<T> SetLerpFunction(ITweener<T>.LerpFunction lerpFunction)
        {
            LerpFunction = lerpFunction;
            return this;
        }

        public ITweener<T> Begin(T from, T to, double durationSeconds)
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

        public async Task Finished()
        {
            await Task.Run(() =>
            {
                while (State.CurrentState != TweenerState.Finished) ;
            });
        }
    }
}

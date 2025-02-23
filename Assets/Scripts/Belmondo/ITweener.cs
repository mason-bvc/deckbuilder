using System;

namespace Belmondo
{
    public interface ITweener<T>
    {
        public delegate void LerpFunction(out T value, T from, T to, double t);
        public void Lerp(out T value, T from, T to, double t);
        public void Update(ref T value, double deltaTime);
        public ITweener<T> Begin(T from, T to, double durationSeconds);
    }
}

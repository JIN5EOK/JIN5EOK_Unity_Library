#if USE_AWAITABLE || USE_UNITASK // Awaitable is supported from Unity 2023.1 , use UniTask for older versions if available
using System;

namespace Jin5eok.Utils.Timer
{
    /// <summary>
    /// Timer that stops at 0 seconds  
    /// </summary>
    public class Timer : ITimer
    {
        public event Action<float> OnLeft;
        public event Action<float> OnTimeover;
        public bool IsRunning => _stopwatch.IsRunning;
        public float LeftSecond => _startSecond - _stopwatch.ElapsedSeconds;
        
        private Stopwatch _stopwatch = new Stopwatch();
        private float _startSecond = 0.0f;
        
        public Timer(float startSecond)
        {
            this._startSecond = startSecond;
            _stopwatch.OnElapsed += Elapsed;
        }

        public void Start() => _stopwatch.Start();

        public void Pause() => _stopwatch.Pause();

        public void Stop() => _stopwatch.Stop();

        private void Elapsed (float elapsed)
        {
            OnLeft?.Invoke(LeftSecond);
            if (LeftSecond < 0.0f)
            {
                var tempLeftSecond = LeftSecond;
                OnTimeover?.Invoke(tempLeftSecond);
                _stopwatch.Stop();
            }
        }
        
        public void Reset()
        {
            OnLeft = null;
            OnTimeover = null;
            _stopwatch.Reset();
            _stopwatch.OnElapsed += Elapsed;
        }
    }
}
#endif
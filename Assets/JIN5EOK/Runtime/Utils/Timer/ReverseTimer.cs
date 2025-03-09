#if USE_AWAITABLE || USE_UNITASK // Awaitable is supported from Unity 2023.1 , use UniTask for older versions if available
using System;

namespace Jin5eok.Utils.Timer
{
    /// <summary>
    /// Timer that stops at the target time  
    /// </summary>
    public class ReverseTimer : ITimer
    {
        public event Action<float> OnElapsed;
        public event Action<float> OnTimeover;
        public bool IsRunning => _stopwatch.IsRunning;
        public float ElapsedSeconds => _stopwatch.ElapsedSeconds;
        
        private Stopwatch _stopwatch = new Stopwatch();
        private float _endSeconds = 0;
        
        public ReverseTimer(float endSeconds)
        {
            this._endSeconds = endSeconds;
            _stopwatch.OnElapsed += Elapsed;
        }
        
        public void Start() => _stopwatch.Start();

        public void Pause() => _stopwatch.Pause();

        public void Stop() => _stopwatch.Stop();

        public void Elapsed(float elapsed)
        {
            OnElapsed?.Invoke(elapsed);
            if (ElapsedSeconds > this._endSeconds)
            {
                var tempElapsedSeconds = elapsed;
                OnTimeover?.Invoke(tempElapsedSeconds);
                _stopwatch.Stop();
            }
        }
        public void Reset()
        {
            OnElapsed = null;
            OnTimeover = null;
            _stopwatch.Reset();
            _stopwatch.OnElapsed += Elapsed;
        }


    }
}
#endif
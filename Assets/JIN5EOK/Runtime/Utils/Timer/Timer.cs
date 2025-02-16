using System;

namespace Jin5eok.Utils.Timer
{
    /// <summary>
    /// 0초가 되면 종료되는 타이머
    /// </summary>
    public class Timer : ITimer
    {
        public bool IsRunning => _stopwatch.IsRunning;
        public event Action<float> OnLeft;
        public event Action<float> OnTimeover;
        
        private Stopwatch _stopwatch = new Stopwatch();
        
        public float LeftSecond => _startSecond - _stopwatch.ElapsedSeconds;
        private float _startSecond = 0.0f;
        public Timer(float startSecond)
        {
            this._startSecond = startSecond;
            _stopwatch.OnElapsed += elapsed =>
            {  
                OnLeft?.Invoke(LeftSecond);
                if (LeftSecond < 0.0f)
                {
                    OnTimeover?.Invoke(LeftSecond);
                    _stopwatch.Stop();
                }
            };
        }

        public void Start() => _stopwatch.Start();

        public void Pause() => _stopwatch.Pause();

        public void Stop() => _stopwatch.Stop();
        
        public void Reset()
        {
            _stopwatch.Reset();
        }
    }
}
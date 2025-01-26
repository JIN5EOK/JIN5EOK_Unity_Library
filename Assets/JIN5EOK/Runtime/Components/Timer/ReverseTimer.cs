using System;

namespace Jin5eok.Timer
{
    /// <summary>
    /// 목표 시간이 되면 종료되는 타이머
    /// </summary>
    public class ReverseTimer : ITimer
    {
        public bool IsRunning => _stopwatch.IsRunning;
        public event Action<float> OnElapsed;
        public event Action<float> OnTimeover;
        
        private Stopwatch _stopwatch = new Stopwatch();
        public float ElapsedSeconds => _stopwatch.ElapsedSeconds;

        private float _endSeconds = 0; // 타이머가 종료될 시간
        public ReverseTimer(float endSeconds)
        {
            this._endSeconds = endSeconds;
            _stopwatch.OnElapsed += elapsed =>
            {
                OnElapsed?.Invoke(elapsed);
                if (ElapsedSeconds > this._endSeconds)
                {
                    OnTimeover?.Invoke(this._endSeconds);
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

using System;
using System.Threading;
using UnityEngine;

namespace Jin5eok.Timer
{
    public class Stopwatch : ITimer
    {
        public event Action<float> OnElapsed; // 주기마다 발생할 이벤트
        public event Action<float> OnStop; // 스톱워치를 멈추면 발생할 이벤트
        public float ElapsedSeconds { get; private set; } // 경과 시간 (초)

        public bool IsRunning
        {
            get;
            private set;
        }
        
        private Awaitable _timerTask = null;
        
        public Stopwatch()
        {
            ElapsedSeconds = 0;
        }

        public void Start()
        {
            if (_timerTask != null)
                return;

            IsRunning = true;
            _timerTask = Run();
        }

        public void Pause()
        {
            if (_timerTask == null)
                return;
            
            _timerTask.Cancel();
            _timerTask = null;
            IsRunning = false;
        }

        public void Stop()
        {
            if (_timerTask == null)
                return;
            
            Pause();
            OnStop?.Invoke(ElapsedSeconds); // 타이머 완료
            ElapsedSeconds = 0;
        }
        
        public void Reset()
        {
            Stop();
            ElapsedSeconds = 0;
        }

        private async Awaitable Run()
        {
            while (IsRunning == true)
            {
                await Awaitable.NextFrameAsync();
                await Awaitable.MainThreadAsync();
                ElapsedSeconds += Time.unscaledDeltaTime;
                OnElapsed?.Invoke(ElapsedSeconds);
            }
        }
    }
}



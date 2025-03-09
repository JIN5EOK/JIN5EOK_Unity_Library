#if USE_AWAITABLE // Awaitable is supported from Unity 2023.1 , use UniTask for older versions if available
using System;
using UnityEngine;

namespace Jin5eok.Utils.Timer
{
    public class Stopwatch : ITimer
    {
        public event Action<float> OnElapsed;
        public event Action<float> OnStop;
        public float ElapsedSeconds { get; private set; }

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
            OnStop?.Invoke(ElapsedSeconds);
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
#elif !UNITY_2023_1_OR_NEWER && USE_UNITASK
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
namespace Jin5eok.Utils.Timer
{
    public class Stopwatch : ITimer
    {
        public event Action<float> OnElapsed;
        public event Action<float> OnStop;
        public float ElapsedSeconds { get; private set; }

        public Stopwatch()
        {
            ElapsedSeconds = 0;
        }

        public bool IsRunning => _cancellationTokenSource != null;
        private CancellationTokenSource _cancellationTokenSource;

        public void Start()
        {
            if (_cancellationTokenSource != null)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            Run(_cancellationTokenSource.Token).Forget();
        }

        public void Pause()
        {
            if (_cancellationTokenSource == null)
                return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Stop()
        {
            Pause();
            OnStop?.Invoke(ElapsedSeconds);
            ElapsedSeconds = 0;
        }

        public void Reset()
        {
            Pause();
            OnElapsed = null;
            OnStop = null;
            ElapsedSeconds = 0;
        }

        private async UniTaskVoid Run(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.NextFrame(cancellationToken);
                await UniTask.SwitchToMainThread();
                ElapsedSeconds += Time.unscaledDeltaTime;
                OnElapsed?.Invoke(ElapsedSeconds);
            }
        }
    }
}
#endif
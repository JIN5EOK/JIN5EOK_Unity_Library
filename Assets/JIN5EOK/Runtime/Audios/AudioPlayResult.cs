using UnityEngine;

namespace Jin5eok.Audios
{
    public struct AudioPlayResult
    {
        public static AudioPlayResult GetSucceedResult(AudioClip audioClip)
        {
            return new AudioPlayResult { Duration = audioClip.length, IsSuccess = true };
        }
        public static AudioPlayResult GetFailedResult()
        {
            return new AudioPlayResult { Duration = -1.0f, IsSuccess = false };
        }
        
        public bool IsSuccess { get; private set; }
        public float Duration { get; private set; }
    }
}
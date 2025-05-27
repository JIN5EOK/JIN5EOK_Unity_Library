using System;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioModel
    {
        public event Action<float> OnVolumeChanged;
        public event Action<float> OnPitchChanged;
        public event Action<bool> OnMuteChanged;
        
        public bool Mute
        {
            get => _mute;
            set
            {
                _mute = value;
                OnMuteChanged?.Invoke(value);
            }
        }
        private bool _mute;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = Mathf.Clamp01(value);
                OnVolumeChanged?.Invoke(_volume);
            }
        }
        private float _volume  = 1f;
        
        public float Pitch
        {
            get => _pitch;
            set
            {
                _pitch = Mathf.Clamp(value,0.0f, float.MaxValue);
                OnPitchChanged?.Invoke(_pitch);
            }
        }
        private float _pitch  = 1f;
    }
}
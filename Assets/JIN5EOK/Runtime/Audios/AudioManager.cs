using Jin5eok.Utils;
using UnityEngine;

namespace Jin5eok.Audios
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSource _oneshotAudioSource;
        
        protected override void Awake()
        {
            base.Awake();
            _oneshotAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        public AudioPlayer GetAudioPlayerInstance(AudioClip audioClip, SoundType soundType)
        {
            var playerObject = new GameObject($"{nameof(AudioPlayer)}/{audioClip.name}");
            playerObject.transform.SetParent(this.transform);
            
            var player = playerObject.AddComponent<AudioPlayer>();
            player.Initialize(audioClip, soundType);
            
            return player;
        }
        
        public AudioPlayResult PlayOneshot(AudioClip clip, SoundType soundType)
        {
            if (clip == null)
            {
                return AudioPlayResult.GetFailedResult();
            }

            var volume = soundType == SoundType.Bgm ? AudioModel.Instance.BgmVolume : AudioModel.Instance.SfxVolume;
            _oneshotAudioSource.PlayOneShot(clip, volume);
            return AudioPlayResult.GetSucceedResult(clip);
        }
    }   
}
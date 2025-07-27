using Jin5eok.Audios;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace Jin5eok.Samples
{
    public class AudioPlayerSample : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _bgmMixer;
        [SerializeField] private AudioMixerGroup _sfxMixer;
        
        private AudioPlayer _bgmPlayer;
        private AudioPlayer _sfxPlayer;
        
        public void CreateBgmPlayer()
        {
            if (_bgmPlayer != null)
                return;

            _bgmPlayer = AudioPlayer.Create(null, _bgmMixer, transform);
            _bgmPlayer.AudioSource.loop = true;
        }
        
        public void CreateSfxPlayer()
        {
            if (_sfxPlayer != null)
                return;

            _sfxPlayer = AudioPlayer.Create(null, _sfxMixer, transform);
        }
        
        public void PlayBgm(AudioClip clip)
        {
            _bgmPlayer.AudioSource.clip = clip;
            _bgmPlayer.Play((result) => Debug.Log($"{clip.name} : { result.ToString()}" ));
        }
        
        public void StopBgm()
        {
            _bgmPlayer.AudioSource.Stop();
        }
        
        public void PlaySfx(AudioClip clip)
        {
            _sfxPlayer.AudioSource.clip = clip;
            _sfxPlayer.Play((result) => Debug.Log($"{clip.name} : { result.ToString()}" ));
        }
        
        public void StopSfx()
        {
            _sfxPlayer.AudioSource.Stop();
        }
        
        public void PlayOneShot(AudioClip clip)
        {
            AudioPlayer.PlayOneShot(clip, null, (result) => Debug.Log($"{clip.name} : { result.ToString()}" ));
        }
    }
}

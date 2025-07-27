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
        
        private AudioPlayerBuilder _audioPlayerBuilder = new ();

        public void CreateBgmPlayer()
        {
            if (_bgmPlayer != null)
                return;

            _bgmPlayer = AudioPlayerManager.Instance.InstantiateAudioPlayer(null, _bgmMixer, transform);
            _bgmPlayer.AudioSource.loop = true;
        }
        
        public void CreateSfxPlayer()
        {
            if (_sfxPlayer != null)
                return;

            _sfxPlayer = _audioPlayerBuilder
                .SetAudioMixerGroup(_sfxMixer)
                .SetTransformParent(transform)
                .Build();
        }
        
        public void PlayBgm(AudioClip clip)
        {
            _bgmPlayer.AudioSource.clip = clip;
            _bgmPlayer.Play((result) => Debug.Log($"Bgm PlayResult : { result.ToString()}" ));
        }
        
        public void StopBgm()
        {
            _bgmPlayer.Stop();
        }
        
        public void PlaySfx(AudioClip clip)
        {
            _sfxPlayer.AudioSource.clip = clip;
            _sfxPlayer.Play((result) => Debug.Log($"Sfx PlayResult : { result.ToString()}" ));
        }
        
        public void StopSfx()
        {
            _sfxPlayer.Stop();
        }
        
        public void PlayOneShot(AudioClip clip)
        {
            AudioPlayerManager.Instance.PlayOneShot(clip, _sfxMixer);
        }
    }
}

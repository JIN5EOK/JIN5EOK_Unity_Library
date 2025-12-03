using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

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
        
        public async void PlayBgm(AudioClip clip)
        {
            if (_bgmPlayer == null)
                return;
                
            _bgmPlayer.AudioSource.clip = clip;
            var result = await _bgmPlayer.PlayAsync();
            Debug.Log($"{clip.name} : {result.ToString()}");
        }
        
        public void StopBgm()
        {
            _bgmPlayer.AudioSource.Stop();
        }
        
        public async void PlaySfxAsync(AudioClip clip)
        {
            if (_sfxPlayer == null)
                return;
                
            _sfxPlayer.AudioSource.clip = clip;
            var result = await _sfxPlayer.PlayAsync();
            Debug.Log($"{clip.name} : {result.ToString()}");
        }
        
        public async void PlaySfxCoroutine(AudioClip clip)
        {
            if (_sfxPlayer == null)
                return;
                
            _sfxPlayer.AudioSource.clip = clip;
            _sfxPlayer.Play(result => Debug.Log($"{clip.name} : {result.ToString()}"));
        }

        public void StopSfx()
        {
            _sfxPlayer.AudioSource.Stop();
        }
        
        public async void PlayOneShot(AudioClip clip)
        {
            var result = await AudioPlayer.PlayOneShotAsync(clip, null);
            Debug.Log($"{clip.name} : {result.ToString()}");
        }
    }
}

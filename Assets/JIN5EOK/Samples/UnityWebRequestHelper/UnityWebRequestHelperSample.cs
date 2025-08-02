using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Jin5eok
{
    public class UnityWebRequestHelperSample : MonoBehaviour
    {
        [Header("Load URL")]
        [SerializeField]
        private string _textURL = "https://file-examples.com/wp-content/storage/2017/02/file_example_JSON_1kb.json";
        [SerializeField]
        private string _textureURL = "https://file-examples.com/wp-content/storage/2017/10/file_example_JPG_100kB.jpg"; 
        [SerializeField]
        private string _audioURL = "https://file-examples.com/wp-content/storage/2017/11/file_example_MP3_700KB.mp3";
        [SerializeField]
        private AudioType _audioType;
        [Space(10)]
        
        [Header("Results")]
        [SerializeField]
        private TMP_Text _targetText;
        [SerializeField]
        private Image _targetImage;
        [SerializeField]
        private AudioSource _targetAudioSource;
        
        public void GetText()
        {
            StartLoadProcess(typeof(string), "Coroutine");
            UnityWebRequestHelper.Get(_textURL, SetText, OnError);
        }

        public async void GetTextAsync()
        {
            StartLoadProcess(typeof(string), "Async Task");
            SetText(await UnityWebRequestHelper.GetAsync(_textURL));
        }
        
        public async void GetTextAwaitableAsync()
        {
            StartLoadProcess(typeof(string), "Async Awaitable");
            SetText(await UnityWebRequestHelper.GetAwaitableAsync(_textURL));
        }
        
        public async void GetTextUniTaskAsync()
        {
            StartLoadProcess(typeof(string), "Async UniTask");
            SetText(await UnityWebRequestHelper.GetUniTaskAsync(_textURL));
        }
        
        public void GetAudio()
        {
            StartLoadProcess(typeof(AudioClip), "Coroutine");
            UnityWebRequestHelper.GetAudioClip(_audioURL,_audioType, SetAudio, OnError);
        }

        public async void GetAudioAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async Task");
            SetAudio(await UnityWebRequestHelper.GetAudioClipAsync(_audioURL, _audioType));
        }
        
        public async void GetAudioUniTaskAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async Awaitable");
            SetAudio(await UnityWebRequestHelper.GetAudioClipUniTaskAsync(_audioURL, _audioType));
        }
        
        public async void GetAudioAwaitableAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async UniTask");
            SetAudio(await UnityWebRequestHelper.GetAudioClipAwaitableAsync(_audioURL, _audioType));
        }
        
        public void GetTexture()
        {
            StartLoadProcess(typeof(Texture2D), "Coroutine");
            UnityWebRequestHelper.GetTexture(_textureURL, SetTexture, OnError);
        }

        public async void GetTextureAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async Task");
            SetTexture(await UnityWebRequestHelper.GetTextureAsync(_textureURL));
        }
        
        public async void GetTextureAwaitableAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async Awaitable");
            SetTexture(await UnityWebRequestHelper.GetTextureAwaitableAsync(_textureURL));
        }
        
        public async void GetTextureUniTaskAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async UniTask");
            SetTexture(await UnityWebRequestHelper.GetTextureUniTaskAsync(_textureURL));
        }

        private void StartLoadProcess(Type loadType, string method)
        {
            _targetImage.gameObject.SetActive(false);
            _targetText.gameObject.SetActive(false);
            _targetAudioSource.Stop();
            Debug.Log($"Start Load {nameof(loadType)} with {method}");
        }
        
        private void SetText(string text)
        {
            _targetText.gameObject.SetActive(true);
            _targetText.text = text;
        }
        
        private void SetTexture(Texture2D texture)
        {
            _targetImage.gameObject.SetActive(true);
            
            if (texture != null)
            {
                _targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);        
            }
        }
        
        private void SetAudio(AudioClip clip)
        {
            _targetAudioSource.clip = clip;
            _targetAudioSource.Play();
        }
        
        private void OnError(UnityWebRequestException exception)
        {
            throw exception;
        }
    }
}
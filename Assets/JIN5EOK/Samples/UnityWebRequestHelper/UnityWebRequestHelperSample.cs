using System;
using System.Threading;
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
        private string _textURL;
        [SerializeField]
        private string _textureURL; 
        [SerializeField]
        private string _audioURL;
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

        private CancellationTokenSource _cancellationToken;
        
        public void GetText()
        {
            StartLoadProcess(typeof(string), "Coroutine");
            UnityWebRequestHelper.Get(_textURL, SetText, OnError, _cancellationToken.Token);
        }

        public async void GetTextAsync()
        {
            StartLoadProcess(typeof(string), "Async Task");
            SetText(await UnityWebRequestHelper.GetAsync(_textURL, _cancellationToken.Token));
        }
        
        public async void GetTextAwaitableAsync()
        {
            StartLoadProcess(typeof(string), "Async Awaitable");
            SetText(await UnityWebRequestHelper.GetAwaitableAsync(_textURL, _cancellationToken.Token));
        }
        
        public async void GetTextUniTaskAsync()
        {
            StartLoadProcess(typeof(string), "Async UniTask");
            SetText(await UnityWebRequestHelper.GetUniTaskAsync(_textURL, _cancellationToken.Token));
        }
        
        public void GetAudio()
        {
            StartLoadProcess(typeof(AudioClip), "Coroutine");
            UnityWebRequestHelper.GetAudioClip(_audioURL,_audioType, SetAudio, OnError, _cancellationToken.Token);
        }

        public async void GetAudioAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async Task");
            SetAudio(await UnityWebRequestHelper.GetAudioClipAsync(_audioURL, _audioType, _cancellationToken.Token));
        }
        
        public async void GetAudioUniTaskAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async Awaitable");
            SetAudio(await UnityWebRequestHelper.GetAudioClipUniTaskAsync(_audioURL, _audioType, _cancellationToken.Token));
        }
        
        public async void GetAudioAwaitableAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async UniTask");
            SetAudio(await UnityWebRequestHelper.GetAudioClipAwaitableAsync(_audioURL, _audioType, _cancellationToken.Token));
        }
        
        public void GetTexture()
        {
            StartLoadProcess(typeof(Texture2D), "Coroutine");
            UnityWebRequestHelper.GetTexture(_textureURL, SetTexture, OnError, _cancellationToken.Token);
        }

        public async void GetTextureAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async Task");
            SetTexture(await UnityWebRequestHelper.GetTextureAsync(_textureURL, _cancellationToken.Token));
        }
        
        public async void GetTextureAwaitableAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async Awaitable");
            SetTexture(await UnityWebRequestHelper.GetTextureAwaitableAsync(_textureURL, _cancellationToken.Token));
        }
        
        public async void GetTextureUniTaskAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async UniTask");
            SetTexture(await UnityWebRequestHelper.GetTextureUniTaskAsync(_textureURL, _cancellationToken.Token));
        }

        private void StartLoadProcess(Type loadType, string method)
        {
            if (_cancellationToken != null)
            {
                Debug.Log($"Request Cancel!");
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
            }
            _cancellationToken = new CancellationTokenSource();
            
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
            Debug.LogError(exception.Message);
        }
    }
}
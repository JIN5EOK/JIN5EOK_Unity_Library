using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        public void GetText()
        {
            StartLoadProcess(typeof(string), "Coroutine");
            UnityWebRequestHelper.Get(_textURL, SetText, OnError);
        }

        public async void GetTextAsync()
        {
            StartLoadProcess(typeof(string), "Async");
            SetText(await UnityWebRequestHelper.GetAsync(_textURL));
        }
        
        public void GetAudio()
        {
            StartLoadProcess(typeof(AudioClip), "Coroutine");
            UnityWebRequestHelper.GetAudioClip(_audioURL,_audioType, SetAudio, OnError);
        }
        
        public async void GetAudioAsync()
        {
            StartLoadProcess(typeof(AudioClip), "Async");
            SetAudio(await UnityWebRequestHelper.GetAudioClipAsync(_audioURL, _audioType));
        }
        
        public void GetTexture()
        {
            StartLoadProcess(typeof(Texture2D), "Coroutine");
            UnityWebRequestHelper.GetTexture(_textureURL, SetTexture, OnError);
        }
        
        public async void GetTextureAsync()
        {
            StartLoadProcess(typeof(Texture2D), "Async");
            SetTexture(await UnityWebRequestHelper.GetTextureAsync(_textureURL));
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
        
        private void OnError(Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }
}
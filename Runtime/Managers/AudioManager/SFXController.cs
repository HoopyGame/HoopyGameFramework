/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：音效管理器
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine.Audio;
using UnityEngine;
using DG.Tweening;

namespace HoopyGame.Manager
{
	public class SFXController
    {
        private const string SFXVolume = "SFXVolume";
        private const float PauseDuration = .5f;

        private AudioSource _audioSource;
        private AudioMixer _audioMixer;
        private AudioConfig _config;

        private float _volume = 1f;
        private bool IsPlaying => _audioSource.isPlaying;
        public SFXController(AudioMixerGroup audioMixerGroup, AudioConfig audioConfig)
        {
            _audioMixer = audioMixerGroup.audioMixer;
            _config = audioConfig;

            new GameObject("SFXController")
                .AddComponent<AudioSource>()
                .transform.SetParent(LSMgr.Instance.transform);
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
            InitData();
        }
        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _audioMixer.SetFloat(SFXVolume, _volume);
            }
        }
        public void InitData()
        {
            Volume = _config.sfxAudioVolume;
        }
        public void Play(AudioClip sfx)
        {
            if (IsPlaying) _audioSource.Stop();
            _audioSource.clip = sfx;
            _audioSource.Play();
        }
        public void PlayOneShot(AudioClip sfx)
        {

        }
        public void Stop()
        {
            _audioSource.Stop();
        }
        public void Pause()
        {
            _audioSource.DOFade(0, PauseDuration)
                .OnComplete(() => { _audioSource.Pause(); });
        }
        public void UnPause()
        {
            _audioSource.DOFade(_volume, PauseDuration)
                .OnComplete(() => { _audioSource.Pause(); });
        }
    }
}
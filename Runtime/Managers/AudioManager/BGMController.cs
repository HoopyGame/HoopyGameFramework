/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：背景音管理
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace HoopyGame.Manager
{
	public class BGMController
	{
		private const string BGMVolume = "BGMVolume";
		private const float PauseDuration = .5f;

        private AudioSource _audioSource;
		private AudioMixer _audioMixer;
        private AudioConfig _config;

        private float _volume = 1f;
		private bool IsPlaying => _audioSource.isPlaying;
		public BGMController(AudioMixerGroup audioMixerGroup,AudioConfig audioConfig)
		{
			_audioMixer = audioMixerGroup.audioMixer;
			_config = audioConfig;

            new GameObject("BGMController")
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
				_audioMixer.SetFloat(BGMVolume, _volume);
            }
		}
		public void InitData()
		{
			Volume = _config.sfxAudioVolume;
		}
		public void Play(AudioClip bgm)
		{
			if (IsPlaying) _audioSource.Stop();
			_audioSource.clip = bgm;
			_audioSource.Play();
		}
		public void ChangeClip(AudioClip bgm)
		{
			_audioSource.DOFade(0, PauseDuration)
				.OnComplete(() =>
				{
					_audioSource.clip = bgm;
					_audioSource.Play();
					_audioSource.DOFade(_volume, PauseDuration);
				});
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
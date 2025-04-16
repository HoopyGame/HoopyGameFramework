/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：音频管理器
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using UnityEngine.Audio;
using VContainer.Unity;
namespace HoopyGame
{
    public class AudioMgr : IStartable
    {
        //这个需要在InitComponent里获取，具体怎么得到这个自己来定
        //默认放在了Resource里，热更请单独提出来
        private AudioMixer _audioMixer;
        private AudioConfig _audioConfig;

        //三个组
        private AudioMixerGroup _bgmAudioMixerGroup;
        private AudioMixerGroup _effAudioMixerGroup;
        private AudioMixerGroup _gameAudioMixerGroup;

        //PlayerPrefs
        private const string BGMAudioVolume = "BGM_Audio_Volume";
        private const string EffAudioVolume = "EFF_Audio_Volume";
        private const string GameAudioVolume = "GAME_Auido_Volume";

        //根据需要自己适配新的Audio
        private AudioSource _bgmAudio;
        private AudioSource _effAudio;

        public void Start()
        {
            DebugUtils.Print("初始化音频管理器..");
            InitComponent();
            InitData();
        }

        //初始化组件
        private void InitComponent()
        {
            //在这里修改加载方式
            _audioMixer = Resources.Load<AudioMixer>("MixerManager");
            _audioConfig = Resources.Load<AudioConfig>("AudioConfig");

            if(_audioMixer)
            {
                _bgmAudioMixerGroup = _audioMixer.FindMatchingGroups("BGM_Group")[0];
                _effAudioMixerGroup = _audioMixer.FindMatchingGroups("Eff_Group")[0];
                _gameAudioMixerGroup = _audioMixer.FindMatchingGroups("Game_group")[0];
            }

            //BGM
            _bgmAudio = CreateAudioObject("BGM_Audio");
            //EffAudio
            _effAudio = CreateAudioObject("EFF_Audio");
            if (_audioMixer != null)
            {
                _bgmAudio.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Bgm_Group")[0];
                _effAudio.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Eff_Group")[0];
            }
        }
        //初始化数据
        private void InitData()
        {
            _bgmAudio.volume = PlayerPrefs.GetFloat(BGMAudioVolume, _audioConfig.bgmAudioVolume);
            _effAudio.volume = PlayerPrefs.GetFloat(EffAudioVolume, _audioConfig.effAudioVolume);
        }

        #region 播放音频
        /// <summary>
        /// 通过一个Clip去播放一个BGM
        /// </summary>
        /// <param name="clip"></param>
        public void PlayBGMAudioByClip(AudioClip clip, bool isLoop)
        {
            _bgmAudio.clip = clip;
            _bgmAudio.loop = isLoop;
            _bgmAudio.Play();
        }
        /// <summary>
        /// 通过一个Clip去播放一个Eff
        /// </summary>
        /// <param name="clip"></param>
        public void PlayEffAudioByClip(AudioClip clip)
        {
            _effAudio.clip = clip;
            _effAudio.Play();
        }
        #endregion
        #region 设置音频大小
        /// <summary>
        /// 设置BGM的音量 0-100
        /// </summary>
        /// <param name="volume"></param>
        public void SetBGMAudioVolume(int volume)
        {
            DebugUtils.Print(AudioVolumeMapping(volume));
        }
        /// <summary>
        /// 设置Eff的音量 0-100
        /// </summary>
        /// <param name="voluem"></param>
        public void SetEffAduioVolume(int voluem)
        {

        }
        //音量映射0-100映射到 - 80-10
        private int AudioVolumeMapping(int volume)
        {
            // 先将0-100线性值转换为0-1
            float linear = volume / 100f;
            // 然后使用对数映射（更符合音频特性）
            return (int)Mathf.Lerp(-80f, 20f, Mathf.Pow(linear, 0.5f));
        }
        #endregion

        /// <summary>
        /// 保存声音大小等参数
        /// </summary>
        public void SaveVolume()
        {
            PlayerPrefs.SetFloat(BGMAudioVolume, _bgmAudio.volume);
            PlayerPrefs.SetFloat(EffAudioVolume, _effAudio.volume);
        }
        private AudioSource CreateAudioObject(string objectName)
        {
            var Aduio = new GameObject(objectName);
            //Aduio.transform.SetParent(transform);
            return Aduio.AddComponent<AudioSource>();
        }
    }
}

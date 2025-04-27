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
namespace HoopyGame.Manager
{
    public class AudioMgr
    {
        //这个需要在InitComponent里获取，具体怎么得到这个自己来定
        //默认放在了Resource里，热更请单独提出来
        private AudioConfig _audioConfig;

        private AudioMixer _audioMixer;
        public AudioMixer Mixer => _audioMixer;
        private AudioListener _audioListener;

        //BGM-SFX
        private BGMController _bgmController;
        private SFXController _sfxConftoller;

        //PlayerPrefs
        private const string BGMAudioVolume = "BGM_Audio_Volume";
        private const string EffAudioVolume = "EFF_Audio_Volume";

        public BGMController BGM => _bgmController;
        public SFXController SFX => _sfxConftoller;

        AudioMgr()
        {
            DebugUtils.Print("初始化音频管理器...");

            InitComponent();
            InitData();
        }

        //初始化组件
        private void InitComponent()
        {
            //在这里修改加载方式
            _audioConfig = new AudioConfig();
            _audioMixer = LSMgr.Instance.GetFromeGLS<AssetMgr>().LoadAssetSync<AudioMixer>("MixerManager");
            _audioListener = Camera.main.GetComponent<AudioListener>();

            _bgmController = new BGMController(_audioMixer.FindMatchingGroups("BGM")[0], _audioConfig);
            _sfxConftoller = new SFXController(_audioMixer.FindMatchingGroups("SFX")[0], _audioConfig);

        }
        //初始化数据
        private void InitData()
        {

        }
        /// <summary>
        /// 保存声音大小等参数
        /// </summary>
        public void SaveVolume()
        {

        }
    }
}

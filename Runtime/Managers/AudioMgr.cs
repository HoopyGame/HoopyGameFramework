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
public class AudioMgr : SingleBaseMono<AudioMgr>
{
    //这个需要在InitComponent里获取，具体怎么得到这个自己来定
    //默认放在了Resource里，热更请单独提出来
    private AudioMixer _audioMixer;

    private const string BGMAudioVolume = "BGM_AUDIO_VOLUME";
    private const string EffAudioVolume = "EFF_AUDIO_VOLUME";

    //根据需要自己适配新的Audio
    private AudioSource _bgmAudio;
    private AudioSource _effAudio;


    private float _bgmAudioVolume = .8f;
    private float _effAudioVolume = .75f;

    public override void OnInit()
    {
        InitComponent();
        InitData();
    }
    //初始化组件
    private void InitComponent()
    {
        //在这里修改加载方式
        _audioMixer = Resources.Load<AudioMixer>("AudioMixer");

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
        _bgmAudioVolume = PlayerPrefs.GetFloat(BGMAudioVolume, .8f);
        _bgmAudio.volume = _bgmAudioVolume;
        _effAudioVolume = PlayerPrefs.GetFloat(EffAudioVolume, .75f);
        _effAudio.volume = _effAudioVolume;
    }

    #region 播放音频
    /// <summary>
    /// 通过一个Clip去播放一个BGM
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBGMAudioByClip(AudioClip clip,bool isLoop)
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
    public void SetBGMAudioVolume(float volume)
    {

    }
    /// <summary>
    /// 设置Eff的音量 0-100
    /// </summary>
    /// <param name="voluem"></param>
    public void SetEffAduioVolume(float voluem)
    {

    }
    //音量映射0-100映射到 -80 -10
    private int AudioVolumeMapping(int volume)
    {
        return (int)(0.8 * volume - 80);
    }
    #endregion

    /// <summary>
    /// 保存声音大小等参数
    /// </summary>
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(BGMAudioVolume, _bgmAudioVolume);
        PlayerPrefs.SetFloat(EffAudioVolume, _effAudioVolume);
    }
    private AudioSource CreateAudioObject(string objectName)
    {
        var Aduio = new GameObject(objectName);
        Aduio.transform.SetParent(transform);
        return Aduio.AddComponent<AudioSource>();
    }
}

/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：按钮的点击效果
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonEffectTools : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerClickHandler
{
    private const int ScaleSize = 1;
    [Header("Data")]
    [SerializeField] private float _maxScaleRatio = 1.02f;
    [SerializeField] private float _minScaleRatio = .97f;
    [SerializeField] private float _scaleDuration = .3f;
    public bool _enableScaleEff = true, _enableAudioEff = true, _isBackBtn = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enableScaleEff)
            SetScaleEff(_maxScaleRatio, _scaleDuration);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_enableScaleEff)
            SetScaleEff(ScaleSize, _scaleDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_enableScaleEff)
            SetScaleEff(_minScaleRatio, _scaleDuration);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_enableScaleEff)
            SetScaleEff(_maxScaleRatio, _scaleDuration);
        if (_enableAudioEff)
            PlayAudioEff(_isBackBtn);

    }

    private void SetScaleEff(float scaleSize, float duration)
    {
        if(this && transform)
            transform.DOScale(scaleSize, duration).SetEase(Ease.OutBack);
    }

    public void PlayAudioEff(bool isBackBtn)
    {
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}

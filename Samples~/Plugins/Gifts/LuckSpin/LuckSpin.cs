/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：幸运转盘礼包
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HoopyGame.UIF;
public class LuckSpin : BasePopup
{
    //奖励
    private LuckSpinItem[] _rewards;
    private int _totalWeight;

    private RectTransform _SpinContent;//用来旋转的

    private Button _start_Btn;

    private int _rotateAngle;//要旋转的角度

    //对旋转的设置
    private int _visualRotateAngle;
    private int _duration;

    public override void OnInitComponent()
    {
        base.OnInitComponent();
        _rewards = new LuckSpinItem[2];
        foreach (var item in _rewards)
        {
            _totalWeight += item.weight;
        }

        _start_Btn = transform.FindButtonFrommChilds("Start_Btn");
        _rotateAngle = 360 / _rewards.Length;
    }
    public override void OnAddListener()
    {
        base.OnAddListener();
        _start_Btn.OnRegister(OnStartBtnClick);
    }
    public override void OnRemoveListener()
    {
        base.OnRemoveListener();
        _start_Btn.OnUnRegister(OnStartBtnClick);
    }

    private void OnStartBtnClick()
    {
        //这里不考虑性能问题，只会抽一次
        int rewardIndex = GetRewardIndexByWeight();
        _visualRotateAngle = Random.Range(6, 10);
        _duration = (int)(_visualRotateAngle / 2);
        var angle = _visualRotateAngle * 360 + rewardIndex * _rotateAngle + Random.Range(-_rotateAngle / 2, _rotateAngle / 2);
        _SpinContent.DOLocalRotate(new Vector3(0, 0, angle), _duration, RotateMode.FastBeyond360).OnComplete(() =>
        {
            GetReward(rewardIndex);
        });
    }

    private int GetRewardIndexByWeight()
    {
        var randomWeightValue = Random.Range(0, _totalWeight);
        var getWeight = 0;
        for (int i = 0; i < _totalWeight; i++)
        {
            getWeight += _rewards[i].weight;
            if (getWeight > randomWeightValue)
            {
                return i;
            }
        }
        return -1;
    }


    private void GetReward(int rewardIndex)
    {
        //获取

    }

}

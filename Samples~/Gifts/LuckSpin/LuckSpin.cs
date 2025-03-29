/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*������ת�����
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using HoopyGame.UIF;
public class LuckSpin : BasePopup
{
    //����
    private LuckSpinItem[] _rewards;
    private int _totalWeight;

    private RectTransform _SpinContent;//������ת��

    private Button _start_Btn;

    private int _rotateAngle;//Ҫ��ת�ĽǶ�

    //����ת������
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
        //���ﲻ�����������⣬ֻ���һ��
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
        //��ȡ

    }

}

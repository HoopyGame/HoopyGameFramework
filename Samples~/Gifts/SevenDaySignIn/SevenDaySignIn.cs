/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：七日签到
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HoopyGame.UIF;
public class SevenDaySignIn : BasePopup
{
    private const string PreviousSignInTime = "PreviousSignInTime_SevenDaySignIn";
    private const string SignInCount = "SignInCount_SevenSignIn";

    //今日，上次签到日期，中间时间间隔
    private DateTime _toDay;
    private DateTime _lastSignInDay;
    private TimeSpan _signInInterval;
    //当前签到次数，最大签到次数
    private short _signInCount;
    private short _maxSignInCount = 7;

    private bool _showNextSignInTime;
    private bool _isMaxSignInCount;

    private Transform _giftToggleParent;
    private Toggle[] _gift_Togs;
    private Button _signIn_Btn;
    private TextMeshProUGUI _signInIntervalTime_Txt;

    private bool _canRepeat;

    public override void OnInitComponent()
    {
        base.OnInitComponent();
        _giftToggleParent = transform.FindTransFromChild("");

        _signIn_Btn = transform.FindButtonFrommChilds("");
    }

    public override void OnAddListener()
    {
        base.OnAddListener();
        _signIn_Btn.OnRegister(OnReceiveRewardBtnClick);

    }
    public override void OnRemoveListener()
    {
        base.OnRemoveListener();
        _signIn_Btn.OnUnRegister(OnReceiveRewardBtnClick);
    }

    public override void OnStart()
    {
        base.OnStart();
        _toDay = DateTime.Now;
        _lastSignInDay = DateTime.Parse(PlayerPrefs.GetString(PreviousSignInTime, DateTime.MinValue.ToString()));
        _signInCount = (short)PlayerPrefs.GetInt(SignInCount, 0);
        if (_signInCount >= _maxSignInCount)
        {
            if (_canRepeat)
            {
                PlayerPrefs.SetInt(SignInCount, 0);
                _signInCount = 0;
            }
        }
        if (CheckCanReceive())
        {
            _signIn_Btn.interactable = true;
            _signInIntervalTime_Txt.text = "签到";
        }
        else
        {
            _showNextSignInTime = true;
            _signIn_Btn.interactable = false;
            
        }
    }
    private void Update()
    {
        if (_showNextSignInTime&& !_isMaxSignInCount)
        {
            _signInInterval = _toDay.AddDays(1).Date - DateTime.Now;

            if (_signInInterval >= TimeSpan.Zero)
            {
                _signInIntervalTime_Txt.text = $"{_signInInterval.Hours}:{_signInInterval.Minutes}:{_signInInterval.Seconds}";
            }
            else
            {
                //可以签到
                _signIn_Btn.interactable = true;
                _signInIntervalTime_Txt.text = "签到";
                _showNextSignInTime = false;
            }
        }
    }

    //领取按钮点击
    public void OnReceiveRewardBtnClick()
    {
        //给奖励
        _signInCount++;
        _lastSignInDay = _toDay;
        PlayerPrefs.SetString(PreviousSignInTime, _lastSignInDay.ToString());
        PlayerPrefs.SetInt(SignInCount, _signInCount);
        //不可以签到
        _showNextSignInTime = true;
        _signIn_Btn.interactable = false;
        UpdateUI();
    }

    public bool CheckCanReceive()
    {
        if (_signInCount >= _maxSignInCount)
        {
            _signIn_Btn.interactable = false;
            _signInIntervalTime_Txt.text = "达到最大签到数";
            _isMaxSignInCount = true;
            return false;
        }
        if (_lastSignInDay.Year <= _toDay.Year && _lastSignInDay.Month <= _toDay.Month && _lastSignInDay.Day < _toDay.Day)
        {
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _signInCount; i++)
        {
            _gift_Togs[i].isOn = true;
        }
    }




}

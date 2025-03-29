/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：观察者模式的事件系统
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using UnityEngine.Events;
public class EventMgr : SingleBase<EventMgr>
{
    enum EventType
    {
        NoParament,
        OneParament
    }
    private Dictionary<string, UnityAction> _noParameterEventDir;
    private Dictionary<string, UnityAction<object>> _oneParameterDir;
    //这里可以根据自己的需求增加新的map

    //确保使用的时候才创建，节省资源
    protected override void Init()
    {
        _noParameterEventDir = new Dictionary<string, UnityAction>();
        _oneParameterDir = new Dictionary<string, UnityAction<object>>();
    }

    #region 没有参数的
    /// <summary>
    /// 添加一个没有参数的订阅
    /// </summary>
    /// <param name="eventName">订阅集的名字</param>
    /// <param name="action">事件</param>
    public void AddEventListener(string eventName, UnityAction action)
    {
        if (HasEvent(eventName, EventType.NoParament))
            _noParameterEventDir[eventName] += action;
        else
            _noParameterEventDir.Add(eventName, action);
    }
    /// <summary>
    /// 移除一个没有参数的订阅
    /// </summary>
    /// <param name="eventName">订阅集的名字</param>
    /// <param name="action">事件</param>
    public void RemoveEventLisTener(string eventName,UnityAction action)
    {
        if (HasEvent(eventName, EventType.NoParament))
            _noParameterEventDir[eventName] -= action;
        else
            DebugUtils.Print("尝试移除一个不存在的无参事件！", DebugType.Error);
    }
    
    /// <summary>
    /// 触发没有参数的事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void TriggerEvent(string eventName)
    {
        if (HasEvent(eventName, EventType.NoParament))
            _noParameterEventDir[eventName]?.Invoke();
        else
            DebugUtils.Print("触发了一个不存在的无参事件！", DebugType.Warning);
    }
    #endregion
    #region 一个参数的
    /// <summary>
    /// 添加一个参数的订阅
    /// </summary>
    /// <param name="eventName">订阅集的名字</param>
    /// <param name="action">事件</param>
    public void AddEventListener(string eventName, UnityAction<object> action)
    {
        if (HasEvent(eventName, EventType.OneParament))
            _oneParameterDir[eventName] += action;
        else
            _oneParameterDir.Add(eventName, action);
    }
    /// <summary>
    /// 移除一个参数的订阅
    /// </summary>
    /// <param name="eventName">订阅集的名字</param>
    /// <param name="action">事件</param>
    public void RemoveEventLisTener(string eventName, UnityAction<object> action)
    {
        if (HasEvent(eventName, EventType.OneParament))
            _oneParameterDir[eventName] -= action;
        else
            DebugUtils.Print("尝试移除一个不存在的1参事件！", DebugType.Error);
    }
    /// <summary>
    /// 触发一个参数的事件
    /// </summary>
    /// <param name="eventName">时间名</param>
    public void TriggerEvent(string eventName,object data)
    {
        if (HasEvent(eventName, EventType.NoParament))
            _oneParameterDir[eventName]?.Invoke(data);
        else
            DebugUtils.Print("触发了一个不存在的有参事件！", DebugType.Warning);
    }

    #endregion

    public override void Clear()
    {
        ClearEvent();
        base.Clear();
    }
    public void ClearEvent()
    {
        _noParameterEventDir.Clear();
        _oneParameterDir.Clear();
    }
    //检测map是否已经包含了此类事件
    private bool HasEvent(string eventName,EventType eventType)
    {
        switch (eventType)
        {
            case EventType.NoParament:
                return _noParameterEventDir.ContainsKey(eventName);
            case EventType.OneParament:
                return _oneParameterDir.ContainsKey(eventName);
                default : return false;
        }
    }

}

/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：扩展-绑定注册
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 17:37:03
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Register
/// </summary>
public static partial class Extension
{
    #region Button

    /// <summary>
    /// 给一个Button添加一个事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnRegister(this Button button, UnityAction action)
    {
        button.onClick.AddListener(action);
    }
    /// <summary>
    /// 给一个Button移除一个事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnUnRegister(this Button button, UnityAction action)
    {
        button.onClick.RemoveListener(action);
    }
    /// <summary>
    /// 移除一个Button所有事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnClear(this Button button)
    {
        button.onClick.RemoveAllListeners();
    }
    /// <summary>
    /// 模拟触发一个Button
    /// </summary>
    /// <param name="button"></param>
    public static void OnTrigger(this Button button)
    {
        button.onClick?.Invoke();
    }
    #endregion
    #region Toggle

    /// <summary>
    /// 给一个Toggle添加一个事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnRegister(this Toggle toggle, UnityAction<bool> action)
    {
        toggle.onValueChanged.AddListener(action);
    }
    /// <summary>
    /// 给一个Toggle移除一个事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnUnRegister(this Toggle toggle, UnityAction<bool> action)
    {
        toggle.onValueChanged.RemoveListener(action);
    }
    /// <summary>
    /// 移除一个Toggle所有事件
    /// </summary>
    /// <param name="action">事件</param>
    public static void OnClaer(this Toggle toggle)
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
    /// <summary>
    /// 模拟点击
    /// </summary>
    /// <param name="button"></param>
    public static void OnTrigger(this Toggle toggle, bool isOn)
    {
        toggle.onValueChanged?.Invoke(isOn);
    }
    #endregion
}
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：扩展-UI
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 17:31:59
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine.EventSystems;

/// <summary>
/// UI
/// </summary>
public static partial class Extension
{
    /// <summary>
    /// 判断是否点击在了UI上
    /// </summary>
    /// <returns>点在了UI上</returns>
    public static bool IsClickUIPointer()
    {
        return EventSystem.current && EventSystem.current.IsPointerOverGameObject();
    }
}
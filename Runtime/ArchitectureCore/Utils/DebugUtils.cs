/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：自定义打印DeBug工具，正式打包去掉日志
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public enum DebugType
{
    Print,
    Warning,
    Error
}
public static class DebugUtils
{
    /// <summary>
    /// Debug帮助类 使用DEBUG_ENABLE宏定义来优化Debug
    /// </summary>
    /// <param name="message">信息</param>
    /// <param name="coloc">自定义颜色，仅在Type为print模式下使用，16进制格式不用加#</param>
    /// <param name="debugType">Debug类型</param>
    [Conditional("DEBUG_ENABLE")]
    public static void Print(object message, DebugType debugType = DebugType.Print, string coloc = "ffffff")
    {
        switch (debugType)
        {
            case DebugType.Print:
                Debug.Log($"<color=#{coloc}>[HG.]{message}</color>");
                break;
            case DebugType.Warning:
                Debug.LogWarning("[HG.] " + message);
                break;
            case DebugType.Error:
                Debug.LogError("[HG.] " + message);
                break;
        }
    }
}

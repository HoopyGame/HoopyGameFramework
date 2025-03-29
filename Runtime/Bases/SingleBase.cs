/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：非Mono单例基类（过时但可用->转为依赖注入）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Threading;
[Obsolete("此基类类已经过时-->请使用依赖注入进行单例管理")]
public class SingleBase<T> where T : SingleBase<T>, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Lazy<T>(() => new T(), LazyThreadSafetyMode.PublicationOnly).Value;
                _instance.Init();
            }
            return _instance;
        }
    }
    [Obsolete]
    public static T GetInstance()
    {
        _instance ??= new T();
        return _instance;
    }
    /// <summary>
    /// 初始化组件、数据（不需要base.Init）
    /// </summary>
    protected virtual void Init() { }
    /// <summary>
    /// 清空组件、数据（请将base.Claer()放在清空所有数据后调用）
    /// </summary>
    public virtual void Clear() { _instance = null; }
}
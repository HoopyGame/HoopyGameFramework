/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：Mono单例基类（过时但可用->转为依赖注入）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using UnityEngine;
//[Obsolete("此基类类已经过时-->请使用依赖注入进行单例管理")]
public class SingleBaseMono<T> : MonoBehaviour where T : SingleBaseMono<T>
{
    protected static bool _inited;

    private static Lazy<T> _instance = new(() => CreateInstance());

    public static T Instance => _instance.Value;

    private static T CreateInstance()
    {
        T instance = FindAnyObjectByType<T>();
        // 如果场景中不存在，则创建一个新的游戏对象
        if (!_inited && instance == null)
        {
            _inited = true;
            // 寻找场景中已经存在的实例
            GameObject singletonObject = new(typeof(T).Name);
            instance = singletonObject.AddComponent<T>();
            instance.OnInit();
            DontDestroyOnLoad(singletonObject);
        }
        return instance;
    }
    /// <summary>
    /// 提供自己初始化自己(避免使用，避免手动挂到场景内)
    /// </summary>
    /// <param name="self">this</param>
    protected void InitSelf(T self)
    {
        if (_inited)
        {
            Destroy(self);
            return;
        }
        _inited = true;
        _instance = new Lazy<T>(self);
        self.OnInit();
        DontDestroyOnLoad(self);
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    public virtual void OnInit() { }
}

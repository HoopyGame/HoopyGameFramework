/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：扩展-查找
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 17:24:27
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Find
/// </summary>
public static partial class Extension
{
    /// <summary>
    /// 通过子物体名字递归查找指定组件
    /// </summary>
    /// <typeparam name="T">要查找的组件</typeparam>
    /// <param name="childName">子物体的名字</param>
    /// <returns></returns>
    public static T FindComponentFromChild<T>(this Transform self, string childName) where T : Component
    {
        Transform child = self.Find(childName);
        if (child != null && child.TryGetComponent(out T childComponent))
        {
            if (childComponent != null) return childComponent;
        }
        foreach (Transform trans in self)
        {
            childComponent = trans.FindComponentFromChild<T>(childName);
            if (childComponent != null) return childComponent;
        }
        return null;
    }

    /// <summary>
    /// 递归查找子物体
    /// </summary>
    /// <param name="childName">子物体的名字</param>
    /// <returns></returns>
    public static Transform FindTransFromChild(this Transform self, string childName)
    {
        Transform child = self.Find(childName);
        if (child != null) return child;
        foreach (Transform trans in self)
        {
            child = trans.FindTransFromChild(childName);
            if (child != null) return child;
        }
        return null;
    }

    /// <summary>
    /// 查找所有场景中是否有某个组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transName"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static T FindComponentFromAllScene<T>(string transName) where T : Component
    {
        GameObject tmp = GameObject.Find(transName);
        if (!tmp) throw new NullReferenceException("没有找该物体");
        if (tmp.TryGetComponent(out T com)) return com;
        else
        {
            //DebugTools.Print($"没有找到 {transName} 物体上的 {typeof(T).Name} 组件 ");
            return null;
        }
    }

    /// <summary>
    /// 获取组件没有就添加
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="gameObject">当前对象</param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var trigger = gameObject.GetComponent<T>();

        if (!trigger)
        {
            trigger = gameObject.AddComponent<T>();
        }

        return trigger;
    }

    #region UI
    /// <summary>
    /// 查找一个Button（省事）
    /// </summary>
    /// <param name="childName">子物体的名字</param>
    /// <returns></returns>
    public static Button FindButtonFrommChilds(this Transform self, string childName)
    {
        return self.FindComponentFromChild<Button>(childName);
    }
    /// <summary>
    /// 查找一个Toggle（省事）
    /// </summary>
    /// <param name="childName">子物体的名字</param>
    /// <returns></returns>
    public static Toggle FindToggleFromChilds(this Transform self, string childName)
    {
        return self.FindComponentFromChild<Toggle>(childName);
    }
    /// <summary>
    /// 查找一个TextMeshProUGUI
    /// </summary>
    /// <param name="self"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static TextMeshProUGUI FindTextMeshProUGUIFromChilds(this Transform self, string childName)
    {
        return self.FindComponentFromChild<TextMeshProUGUI>(childName);
    }
    #endregion
}
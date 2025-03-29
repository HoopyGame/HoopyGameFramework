/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：使用Resources方式加载UI的工厂
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using HoopyGame.UIF;

public class LoadUIByResource : ILoadUI
{
    public BaseUI LoadUI(string UIName, Transform uiParent)
    {
        if (string.IsNullOrEmpty(UIName)) throw new ArgumentException("UIName 为空");
        GameObject uiObj = Resources.Load<GameObject>(UIName);
        if (!uiObj) throw new MissingReferenceException("加载了不存在的资源！" + UIName);
        GameObject go = GameObject.Instantiate(uiObj, uiParent, false);
        go.name = UIName;
        if (!go.TryGetComponent(out BaseUI baseUI)) throw new MissingComponentException("对象上没有挂载或继承BaseUI组件!" + UIName);
        return baseUI;
    }
    public async UniTask<BaseUI> LoadUIAsync(string UIName, Transform uiParent)
    {
        if (string.IsNullOrEmpty(UIName)) throw new ArgumentException("UIName 为空");

        GameObject uiObj = await Resources.LoadAsync<GameObject>(UIName) as GameObject;
        if (!uiObj) throw new MissingReferenceException("加载了不存在的资源！" + UIName);
        GameObject go = GameObject.Instantiate(uiObj, uiParent, false);
        go.name = UIName;
        if (!go.TryGetComponent(out BaseUI baseUI)) throw new MissingComponentException("对象上没有挂载或继承BaseUI组件!" + UIName);
        return baseUI;
    }
}

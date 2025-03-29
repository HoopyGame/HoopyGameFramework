/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：加载UI的接口
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using Cysharp.Threading.Tasks;
using UnityEngine;
using HoopyGame.UIF;

public interface ILoadUI
{
    /// <summary>
    /// 同步加载 非必要不使用
    /// </summary>
    /// <param name="UIName">UI名字</param>
    /// <param name="uiParent">UI的父节点</param>
    /// <returns></returns>
    public BaseUI LoadUI(string UIName, Transform uiParent);
    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="UIName">UI名字</param>
    /// <param name="uiParent">UI的父节点</param>
    /// <returns></returns>
    public UniTask<BaseUI> LoadUIAsync(string UIName, Transform uiParent);

}

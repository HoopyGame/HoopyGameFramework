/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：创建UI的工厂
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

public class UIFactory : ILoadUI
{
    //工厂接口
    private readonly ILoadUI _loadUIFactory;
    public UIFactory()
    {
        //需要什么方式加载UI，就在这里改
        _loadUIFactory = new LoadUIByResource();
    }

    public BaseUI LoadUI(string UIName, Transform uiParent)
    {
        return _loadUIFactory.LoadUI(UIName, uiParent);
    }

    public async UniTask<BaseUI> LoadUIAsync(string UIName, Transform uiParent)
    {
        return await _loadUIFactory.LoadUIAsync(UIName, uiParent);
    }
}

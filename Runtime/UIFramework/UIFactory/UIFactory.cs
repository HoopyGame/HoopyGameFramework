/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*������UI�Ĺ���
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using Cysharp.Threading.Tasks;
using UnityEngine;
using HoopyGame.UIF;

public class UIFactory : ILoadUI
{
    //�����ӿ�
    private readonly ILoadUI _loadUIFactory;
    public UIFactory()
    {
        //��Ҫʲô��ʽ����UI�����������
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

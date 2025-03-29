/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：连招模块->每一个基本Key
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine;

/// <summary>
/// 搓招命令
/// </summary>
public class ComboCommandkey
{
    public float time;
    public float timer;
    public KeyCode commandKey;

    public ComboCommandkey(KeyCode commandKey,float interval=.5f)
    {
        time = interval;
        timer = time;
        this.commandKey = commandKey;
    }
    /// <summary>
    /// 进入下一个命令状态
    /// </summary>
    public void Enter()
    {
        timer = time;
    }
    public void Update(ComboCommandManager ccm)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            time = timer;
            //reset
            ccm.Reset();
        }
        else
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(commandKey))
                {
                    //下一步
                    ccm.Next();
                }
                else
                {
                    //reset
                    ccm.Reset();
                }
            }

        }
    }

}
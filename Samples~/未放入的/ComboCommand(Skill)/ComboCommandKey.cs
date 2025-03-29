/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*������ģ��->ÿһ������Key
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/

using UnityEngine;

/// <summary>
/// ��������
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
    /// ������һ������״̬
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
                    //��һ��
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
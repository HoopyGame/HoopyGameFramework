/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：连招模块管理器控制触发时机和初始化
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
public class ComboCommandManager
{
    public string skillName;
    public ComboCommandkey[] comboKeys;
    public ComboTrigger comboTrigger;

    private int currentComboKeyIndex;

    public ComboCommandManager(string skillName,ComboTrigger comboTrigger, params ComboCommandkey[] comboKeys)
    {
        this.skillName = skillName;
        this.comboKeys = comboKeys;
        this.comboTrigger = comboTrigger;
        currentComboKeyIndex = 0;
    }

    public void Next()
    {
        currentComboKeyIndex++;
        if(currentComboKeyIndex >= comboKeys.Length)
        {
            //搓招成功
            currentComboKeyIndex = 0;
        }
        comboKeys[currentComboKeyIndex].Enter();
    }
    public void Update()
    {
        comboKeys[currentComboKeyIndex].Update(this);
    }
    public void Reset()
    {
        currentComboKeyIndex = 0;
    }
}

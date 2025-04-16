/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：红点的抽象基类
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
public abstract class RedPointModelBase
{
    /// <summary>
    /// 红点名称
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 红点类型
    /// </summary>
    public RedPointType RedPointType { get; private set; }

    public RedPointModelBase(string name, RedPointType redPointType)
    {
        Name = name;
        RedPointType = redPointType;
    }
    /// <summary>
    /// 当红点数据发生变化后调用此方法
    /// </summary>
    public abstract void OnRedPointValueChangeCallBack(int value);
}

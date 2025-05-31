/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：红点的节点
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
/// <summary>
/// 树节点
/// </summary>
[Serializable]
public class RedPointNode
{
    /// <summary>
    /// 子节点
    /// </summary>
    public Dictionary<string, RedPointNode> ChildMap
    {
        get;
        private set;
    }
    /// <summary>
    /// 节点值改变回调
    /// </summary>
    private Action<int> _valueChangeCallback;
    /// <summary>
    /// 节点名
    /// </summary>
    public string Name
    {
        get;
        private set;
    }
    /// <summary>
    /// 节点值
    /// </summary>
    public int Value
    {
        get;
        private set;
    }
    /// <summary>
    /// 父节点
    /// </summary>
    public RedPointNode Parent
    {
        get;
        private set;
    }

    /// <summary>
    /// 完整路径
    /// </summary>
    private string fullPath;
    /// <summary>
    /// 完整路径
    /// </summary>
    public string FullPath
    {
        get
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                if (Parent == null || Parent == RedPointManager.Instance.Root)
                {
                    fullPath = Name;
                }
                else
                {
                    fullPath = Parent.FullPath + RedPointManager.Instance.SplitChar + Name;
                }
            }

            return fullPath;
        }
    }

    /// <summary>
    /// 子节点数量
    /// </summary>
    public int ChildrenCount => ChildMap.Count;

    public RedPointNode()
    {
        ChildMap = new Dictionary<string, RedPointNode>();
    }

    public void Init(string name, RedPointNode parent)
    {
        Name = name;
        Parent = parent;
    }

    /// <summary>
    /// 添加节点值监听
    /// </summary>
    public void AddListener(Action<int> callback)
    {
        _valueChangeCallback += callback;
    }

    /// <summary>
    /// 移除节点值监听
    /// </summary>
    public void RemoveListener(Action<int> callback)
    {
        _valueChangeCallback -= callback;
        
    }
    /// <summary>
    /// 移除所有节点值监听
    /// </summary>
    public void RemoveAllListener()
    {
        _valueChangeCallback = null;
    }

    /// <summary>
    /// 改变节点值（使用传入的新值，只能在叶子节点上调用）
    /// </summary>
    public void ChangeValue(int newValue)
    {
        if (ChildrenCount != 0)
        {
            throw new Exception("不允许直接改变非叶子节点的值：" + FullPath);
        }
        InternalChangeValue(newValue);
    }

    /// <summary>
    /// 改变节点值（根据子节点值计算新值，只对非叶子节点有效）
    /// </summary>
    public void ChangeValue()
    {
        int sum = 0;

        if (ChildrenCount != 0)
        {
            foreach (var child in ChildMap)
            {
                sum += child.Value.Value;
            }
        }

        InternalChangeValue(sum);
    }

    /// <summary>
    /// 获取子节点，如果不存在则添加
    /// </summary>
    public RedPointNode GetOrAddChild(string name)
    {
        GetChild(name,out RedPointNode childNode);
        return childNode ?? AddChild(name);
    }

    /// <summary>
    /// 获取子节点
    /// </summary>
    public bool GetChild(string nodeName,out RedPointNode childNode)
    {
        bool found = ChildMap.TryGetValue(nodeName, out RedPointNode child);
        if (found) childNode = child;
        else childNode = null;
        return found;
    }

    /// <summary>
    /// 添加子节点
    /// </summary>
    public RedPointNode AddChild(string nodeName)
    {
        if (ChildMap.ContainsKey(nodeName))
        {
            throw new Exception("子节点添加失败，不允许重复添加：" + FullPath);
        }

        RedPointNode child = new ();
        child.Init(nodeName, this);
        ChildMap.Add(nodeName, child);
        RedPointManager.Instance.NodeNumChangeCallback?.Invoke();
        return child;
    }

    /// <summary>
    /// 移除子节点
    /// </summary>
    public bool RemoveChild(string nodeName)
    {
        if  (ChildrenCount == 0) return false;

        if(GetChild(nodeName, out RedPointNode child))
        {
            //子节点被删除 需要进行一次父节点刷新
            RedPointManager.Instance.MarkDirtyNode(this);

            ChildMap.Remove(nodeName);

            RedPointManager.Instance.NodeNumChangeCallback?.Invoke();

            return true;
        }
        return false;
    }

    /// <summary>
    /// 移除所有子节点
    /// </summary>
    public void RemoveAllChild()
    {
        if (ChildMap.Count == 0) return;

        ChildMap.Clear();
        RedPointManager.Instance.MarkDirtyNode(this);
        RedPointManager.Instance.NodeNumChangeCallback?.Invoke();
    }

    /// <summary>
    /// 改变节点值
    /// </summary>
    private void InternalChangeValue(int newValue)
    {
        if (Value == newValue) return;

        //更新节点值
        Value = newValue;

        _valueChangeCallback?.Invoke(Value);
        RedPointManager.Instance.NodeValueChangeCallback?.Invoke(this, Value);

        //标记父节点为脏节点
        RedPointManager.Instance.MarkDirtyNode(Parent);

        //更新值
        RedPointManager.Instance.Update();
    }
}

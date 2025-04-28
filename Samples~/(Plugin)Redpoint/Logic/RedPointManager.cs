/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：基于前缀树的红点系统
│　使用说明： 先在RedPointName中添加红点的Const Path(string)
|           获取红点组件(RedPointItem)调用内部Init方法
|           使用RedPointManager.ChangeValue()来更新红点,Update()刷新数据
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
public class RedPointManager : SingleBase<RedPointManager>
{
    /// <summary>
    /// 所有节点集合
    /// </summary>
    private Dictionary<string, RedPointNode> m_AllNodes;

    /// <summary>
    /// 脏节点集合
    /// </summary>
    private HashSet<RedPointNode> m_DirtyNodes;

    /// <summary>
    /// 临时脏节点集合
    /// </summary>
    private List<RedPointNode> m_TempDirtyNodes;

    /// <summary>
    /// 节点数量改变回调
    /// </summary>
    public Action NodeNumChangeCallback;

    /// <summary>
    /// 节点值改变回调
    /// </summary>
    public Action<RedPointNode,int> NodeValueChangeCallback;

    /// <summary>
    /// 路径分隔字符
    /// </summary>
    public readonly char SplitChar = '_';

    /// <summary>
    /// 缓存的每个节点的路径
    /// </summary>
    public List<string> CachedSb
    {
        get;
        private set;
    }

    /// <summary>
    /// 红点树根节点
    /// </summary>
    public RedPointNode Root
    {
        get;
        private set;
    }
    protected override void Init()
    {
        base.Init();
        m_AllNodes = new Dictionary<string, RedPointNode>();
        Root = new RedPointNode();
        Root.Init(RedPointName.Root, null);
        m_AllNodes.Add(RedPointName.Root, Root);

        m_DirtyNodes = new HashSet<RedPointNode>();
        m_TempDirtyNodes = new List<RedPointNode>();
        CachedSb = new List<string>();
    }

    /// <summary>
    /// 添加节点值监听
    /// </summary>
    public RedPointNode AddListener(string path,Action<int> callback)
    {
        if (callback == null)
            return null;

        RedPointNode node = GetTreeNode(path);
        node.AddListener(callback);
        return node;
    }

    /// <summary>
    /// 移除节点值监听
    /// </summary>
    public void RemoveListener(string path,Action<int> callback)
    {
        if (callback == null) return;
        RedPointNode node = GetTreeNode(path);
        node.RemoveListener(callback);
    }

    /// <summary>
    /// 移除所有节点值监听
    /// </summary>
    public void RemoveAllListener(string path)
    {
        RedPointNode node = GetTreeNode(path);
        node.RemoveAllListener();
    }

    /// <summary>
    /// 改变节点值
    /// </summary>
    public void ChangeValue(string path,int newValue)
    {
        RedPointNode node = GetTreeNode(path);
        node.ChangeValue(newValue);
    }

    /// <summary>
    /// 获取节点值
    /// </summary>
    public int GetValue(string path)
    {
        RedPointNode node = GetTreeNode(path);
        if (node == null)
        {
            return 0;
        }

        return node.Value;
    }

    /// <summary>
    /// 获取节点
    /// </summary>
    public RedPointNode GetTreeNode(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception("路径不合法，不能为空");
        }
        if (m_AllNodes.TryGetValue(path,out RedPointNode node))
        {
            return node;
        }


        RedPointNode cur = Root;
        CachedSb.Clear();
        CachedSb.AddRange(path.Split(SplitChar));

        foreach (var item in CachedSb)
        {
            RedPointNode child = cur.GetOrAddChild(item);
            cur = child;
        }

        m_AllNodes.Add(path, cur);

        return cur;
    }

    /// <summary>
    /// 移除节点
    /// </summary>
    public bool RemoveTreeNode(string path)
    {
        if (!m_AllNodes.ContainsKey(path))
            return false;

        RedPointNode node = GetTreeNode(path);
        m_AllNodes.Remove(path);
        return node.Parent.RemoveChild(node.Name);
    }

    /// <summary>
    /// 移除所有节点
    /// </summary>
    public void RemoveAllTreeNode()
    {
        Root.RemoveAllChild();
        m_AllNodes.Clear();
    }

    /// <summary>
    /// 管理器轮询
    /// </summary>
    public void Update()
    {
        if (m_DirtyNodes.Count == 0)
        {
            return;
        }
        m_TempDirtyNodes.Clear();
        m_TempDirtyNodes.AddRange(m_DirtyNodes);

        //处理所有脏节点
        for (int i = 0; i < m_TempDirtyNodes.Count; i++)
        {
            m_TempDirtyNodes[i].ChangeValue();
        }
    }

    /// <summary>
    /// 标记脏节点
    /// </summary>
    public void MarkDirtyNode(RedPointNode node)
    {
        if (node == null || node.Name == Root.Name)
        {
            return;
        }

        m_DirtyNodes.Add(node);
        //这里要标记每个父类都为脏节点
        MarkDirtyNode(node.Parent);
    }

}
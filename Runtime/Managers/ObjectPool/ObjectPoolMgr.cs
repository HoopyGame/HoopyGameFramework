/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：基于ObjectPool的对象池（车间的概念）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr
{
    //所有对象池的父物体
    private Transform _allPoolDataParent = null;
    private Dictionary<string, SinglePoolData> _objectPool;

    ObjectPoolMgr()
    {
        _objectPool = new Dictionary<string, SinglePoolData>();
        _allPoolDataParent = new GameObject("GameObjectPool").transform;
    }
    /// <summary>
    /// 从池子内拿取
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Pull(string name)
    {
        CheckHasSinglePoolData(name);
        return _objectPool[name].Pool.Get();
    }
    /// <summary>
    /// 通过物体名字（地址）物体放入池子内
    /// </summary>
    /// <param name="item">物体</param>
    public void Push(GameObject item)
    {
        CheckHasSinglePoolData(item.name);
        _objectPool[item.name].Pool.Release(item);
    }
    /// <summary>
    /// 检测是否有对应的小池子
    /// </summary>
    /// <param name="path"></param>
    public void CheckHasSinglePoolData(string path)
    {
        if (!_objectPool.ContainsKey(path))
        {
            CreateSinglePoolData(_allPoolDataParent, path);
        }
    }
    /// <summary>
    /// 创建一个单个小池子
    /// </summary>
    /// <param name="allPoolParent"></param>
    /// <param name="itenName"></param>
    public void CreateSinglePoolData(Transform allPoolParent, string itenName)
    {
        _objectPool.Add(itenName, new SinglePoolData(allPoolParent, itenName));
    }
    /// <summary>
    /// 清空池子
    /// </summary>
    public void Clear()
    {
        _objectPool.Clear();
    }
}


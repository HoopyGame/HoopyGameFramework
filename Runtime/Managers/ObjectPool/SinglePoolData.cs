/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：单个池子的数据（流水线的概念）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using UnityEngine.Pool;

public class SinglePoolData
{
    public ObjectPool<GameObject> Pool;
    private Transform _parent;
    private string _fullPath;
    public SinglePoolData(Transform allPoolParent, string fullPath)
    {
        _parent = new GameObject(fullPath).transform;
        _parent.SetParent(allPoolParent);
        _fullPath = fullPath;

        Pool = new ObjectPool<GameObject>(Create, Get, Hide, Release, true, 10, 50);
    }
    /// <summary>
    /// 当池子内不够资源的时候创建
    /// </summary>
    /// <returns></returns>
    private GameObject Create()
    {
        GameObject item = GameObject.Instantiate(Resources.Load<GameObject>(_fullPath));
        item.name = _fullPath;
        return item;
    }
    /// <summary>
    /// 当池子内有资源的时候直接拿取
    /// </summary>
    /// <param name="obj"></param>
    private void Get(GameObject obj)
    {
        obj.SetActive(true);
    }
    /// <summary>
    /// 将物体放入池子内
    /// </summary>
    /// <param name="obj"></param>
    private void Hide(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(_parent);
    }
    /// <summary>
    /// 将物体彻底销毁
    /// </summary>
    /// <param name="obj"></param>
    private void Release(GameObject obj)
    {
        //可寻址的话这里Release掉
    }
}

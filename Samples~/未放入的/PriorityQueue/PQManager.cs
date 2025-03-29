/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：小顶堆的优先级队列
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;

/// <summary>
/// 用于管理所有的优先级队列(小顶堆)
/// </summary>
/// <typeparam name="IPQ"></typeparam>
public class PQManager<T> where T : PQBase
{
    //节点类型
    public enum NodeType
    {
        Left = 1,
        Right = 2
    }

    public List<T> PQBaseList;
    /// <summary>
    /// 当前队列中有多少个
    /// </summary>
    public int Count { get => PQBaseList.Count; }
    /// <summary>
    /// 最后节点的索引
    /// </summary>
    public int EndIndex { get => Count - 1; }
    
    public PQManager(int defaultSize)
    {
        PQBaseList = new List<T>(defaultSize);
    }


    /// <summary>
    /// 放入一个数据到队列中
    /// </summary>
    /// <param name="pQBase">元素</param>
    public void Enqueue(T pQBase)
    {
        PQBaseList.Add(pQBase);
        var currentIndex = EndIndex;
        var parrentIndex = GetParentIndex(currentIndex);

        while (currentIndex > 0 && PQBaseList[currentIndex].CompareTo(PQBaseList[parrentIndex]) < 0)
        {
            SwapData(currentIndex, parrentIndex);
            currentIndex = parrentIndex;
            parrentIndex = GetParentIndex(currentIndex);
        }
    }
    /// <summary>
    /// 取出最优先的一个元素,没有元素则为null
    /// </summary>
    /// <returns>元素</returns>
    public T DeQueue()
    {
        if (Count <= 0) return null;
        var result = PQBaseList[0];
        var recordEndIndex = EndIndex;
        PQBaseList[0] = PQBaseList[recordEndIndex];
        PQBaseList.RemoveAt(recordEndIndex);
        recordEndIndex--;

        var topIndex = 0;
        int childIndex;
        var currentMinElementIndex = topIndex;//记录每次循环内最小的索引
        while (true)
        {
            //将top元素与左边元素作对比获取其中最小的
            childIndex = GetChildIndex(topIndex, NodeType.Left);
            if (childIndex <= recordEndIndex && PQBaseList[childIndex].CompareTo(PQBaseList[topIndex]) < 0)
            {
                currentMinElementIndex = childIndex;
            }
            //再将右边的元素与其对比获取最小的
            childIndex = GetChildIndex(topIndex, NodeType.Right);
            if (childIndex <= recordEndIndex && PQBaseList[childIndex].CompareTo(PQBaseList[currentMinElementIndex]) < 0)
            {
                currentMinElementIndex = childIndex;
            }
            //判断完成后查看是否交换
            if (currentMinElementIndex == topIndex) break;
            SwapData(currentMinElementIndex, topIndex);
            topIndex = currentMinElementIndex;
        }
        return result;
    }

    //交换数据
    private void SwapData(int indexA, int indexB)
    {
        (PQBaseList[indexA], PQBaseList[indexB]) = (PQBaseList[indexB], PQBaseList[indexA]);
    }

    /// <summary>
    /// 获取一个元素，若不存在则返回null
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>元素</returns>
    public T Pick(int index)
    {
        return index >= 0 && index < Count ? PQBaseList[index] : null;
    }
    /// <summary>
    /// 获取元素索引
    /// </summary>
    /// <param name="pQBase">元素</param>
    /// <returns>索引</returns>
    public int IndexOf(T pQBase)
    {
        return PQBaseList.IndexOf(pQBase);
    }
    /// <summary>
    /// 获取当前节点索引的父节点
    /// </summary>
    /// <param name="currentIndex">当前节点</param>
    /// <returns>若队列为空或查询节点超出队列元素数量返回-1,Top节点则返回0</returns>
    public int GetParentIndex(int currentIndex)
    {
        return currentIndex <= 0 ? 0 : (currentIndex - 1) / 2;
        
    }
    /// <summary>
    /// 获取当前节点索引的子节点
    /// </summary>
    /// <param name="currentIndex">当前节点</param>
    /// <returns>子节点索引</returns>
    public int GetChildIndex(int currentIndex,NodeType nodeType)
    {
        return currentIndex * 2 + (int)nodeType;
    }

}

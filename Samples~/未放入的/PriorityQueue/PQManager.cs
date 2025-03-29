/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*��С���ѵ����ȼ�����
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using System.Collections.Generic;

/// <summary>
/// ���ڹ������е����ȼ�����(С����)
/// </summary>
/// <typeparam name="IPQ"></typeparam>
public class PQManager<T> where T : PQBase
{
    //�ڵ�����
    public enum NodeType
    {
        Left = 1,
        Right = 2
    }

    public List<T> PQBaseList;
    /// <summary>
    /// ��ǰ�������ж��ٸ�
    /// </summary>
    public int Count { get => PQBaseList.Count; }
    /// <summary>
    /// ���ڵ������
    /// </summary>
    public int EndIndex { get => Count - 1; }
    
    public PQManager(int defaultSize)
    {
        PQBaseList = new List<T>(defaultSize);
    }


    /// <summary>
    /// ����һ�����ݵ�������
    /// </summary>
    /// <param name="pQBase">Ԫ��</param>
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
    /// ȡ�������ȵ�һ��Ԫ��,û��Ԫ����Ϊnull
    /// </summary>
    /// <returns>Ԫ��</returns>
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
        var currentMinElementIndex = topIndex;//��¼ÿ��ѭ������С������
        while (true)
        {
            //��topԪ�������Ԫ�����ԱȻ�ȡ������С��
            childIndex = GetChildIndex(topIndex, NodeType.Left);
            if (childIndex <= recordEndIndex && PQBaseList[childIndex].CompareTo(PQBaseList[topIndex]) < 0)
            {
                currentMinElementIndex = childIndex;
            }
            //�ٽ��ұߵ�Ԫ������ԱȻ�ȡ��С��
            childIndex = GetChildIndex(topIndex, NodeType.Right);
            if (childIndex <= recordEndIndex && PQBaseList[childIndex].CompareTo(PQBaseList[currentMinElementIndex]) < 0)
            {
                currentMinElementIndex = childIndex;
            }
            //�ж���ɺ�鿴�Ƿ񽻻�
            if (currentMinElementIndex == topIndex) break;
            SwapData(currentMinElementIndex, topIndex);
            topIndex = currentMinElementIndex;
        }
        return result;
    }

    //��������
    private void SwapData(int indexA, int indexB)
    {
        (PQBaseList[indexA], PQBaseList[indexB]) = (PQBaseList[indexB], PQBaseList[indexA]);
    }

    /// <summary>
    /// ��ȡһ��Ԫ�أ����������򷵻�null
    /// </summary>
    /// <param name="index">����</param>
    /// <returns>Ԫ��</returns>
    public T Pick(int index)
    {
        return index >= 0 && index < Count ? PQBaseList[index] : null;
    }
    /// <summary>
    /// ��ȡԪ������
    /// </summary>
    /// <param name="pQBase">Ԫ��</param>
    /// <returns>����</returns>
    public int IndexOf(T pQBase)
    {
        return PQBaseList.IndexOf(pQBase);
    }
    /// <summary>
    /// ��ȡ��ǰ�ڵ������ĸ��ڵ�
    /// </summary>
    /// <param name="currentIndex">��ǰ�ڵ�</param>
    /// <returns>������Ϊ�ջ��ѯ�ڵ㳬������Ԫ����������-1,Top�ڵ��򷵻�0</returns>
    public int GetParentIndex(int currentIndex)
    {
        return currentIndex <= 0 ? 0 : (currentIndex - 1) / 2;
        
    }
    /// <summary>
    /// ��ȡ��ǰ�ڵ��������ӽڵ�
    /// </summary>
    /// <param name="currentIndex">��ǰ�ڵ�</param>
    /// <returns>�ӽڵ�����</returns>
    public int GetChildIndex(int currentIndex,NodeType nodeType)
    {
        return currentIndex * 2 + (int)nodeType;
    }

}

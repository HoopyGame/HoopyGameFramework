/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：扩展-通用
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 17:31:59
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine;

#region 目前有的功能
/// <summary>
/// (给Button,Toggle等组件扩展添加时间方法)
/// 获取当前时间戳
/// 通过子物体名字递归查找指定组件
/// 递归查找子物体
/// 查找所有场景中是否有某个组件
/// 将一个Texture2D转换成Sprite
/// 返回一个Texture2D完整的Rect
/// 判断是否点击在了UI上
/// 移动物体Sibling
/// </summary>
#endregion
public static partial class Extension
{
    #region 枚举
    //移动方向枚举
    public enum SiblingOpt
    {
        MoveOffset,
        Top,
        Bottom
    }
    #endregion
    
    /// <summary>
    /// 将一个Texture2D转换成Sprite
    /// </summary>
    /// <param name="texture2D">Texture</param>
    /// <param name="rect">正常为Texture</param>
    /// <param name="pivot"></param>
    /// <returns></returns>
    public static Sprite Texture2DToSprite(Texture2D texture2D, Rect rect, Vector2 pivot)
    {
        return Sprite.Create(texture2D, rect, pivot);
    }

    /// <summary>
    /// 返回一个Texture2D完整的Rect
    /// </summary>
    /// <param name="texture2D"></param>
    /// <returns></returns>
    public static Rect GetTexture2DRect(Texture2D texture2D)
    {
        return new Rect(0, 0, texture2D.width, texture2D.height);
    }
    
    /// <summary>
    /// 移动物体在Hierarchy上的Sibling（层级）
    /// </summary>
    /// <param name="siblingOpt">如何移动</param>
    /// <param name="moveNum">移动多少(默认为1)，T，B无效</param>
    public static void ChangeTransSibling(this Transform trans, SiblingOpt siblingOpt, int offect)
    {
        switch (siblingOpt)
        {
            case SiblingOpt.MoveOffset:
                //这里防止了到-1导致到顶层的问题，若需要循环可自行注释掉
                if (offect < 0)
                {
                    if (trans.GetSiblingIndex() != 0)
                        trans.SetSiblingIndex(trans.GetSiblingIndex() + offect);
                }
                else
                    trans.SetSiblingIndex(trans.GetSiblingIndex() + offect);
                break;
            case SiblingOpt.Top:
                trans.SetAsLastSibling();
                break;
            case SiblingOpt.Bottom:
                trans.SetAsFirstSibling();
                break;
        }
    }

    /// <summary>
    /// 交换两个数据
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    public static void Swap(ref object A, ref object B)
    {
        (B, A) = (A, B);
    }
}

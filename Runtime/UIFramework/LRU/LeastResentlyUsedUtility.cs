/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：最近最久未使用基础算法，N次不操作UI就清除掉
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
using HoopyGame.Core;

namespace HoopyGame.UIF
{
    public class LeastResentlyUsedUtility : IUtility
    {
        private class Content
        {
            /// <summary>
            /// 当前UI
            /// </summary>
            public BaseUI baseUI;
            /// <summary>
            /// 多久没有操作当前UI了
            /// </summary>
            public int nurCount;
        }

        private Dictionary<string, Content> _hideUiMap;                           //关闭但是没有销毁的表
        private readonly List<string> closedUIAfter;
        private readonly int _closeUIThreshold;                                   //多少次未再次打开就销毁
        private BaseUI tmpBaseui;


        public LeastResentlyUsedUtility(int closeUIThreshold)
        {
            _closeUIThreshold = closeUIThreshold;
            tmpBaseui = null;
            _hideUiMap = new Dictionary<string, Content>();
            closedUIAfter = new List<string>();
        }
        /// <summary>
        /// 将一个UI从关闭但未销毁(仅隐藏)的列表内移除
        /// </summary>
        /// <param name="baseUI"></param>
        public void RemoveFromTotalHideUIList(string uiName)
        {
            _hideUiMap.Remove(uiName);
        }

        /// <summary>
        /// 将一个UI放入关闭但未销毁(仅隐藏)的列表内
        /// </summary>
        /// <param name="baseUI"></param>
        public void AddToTotalHideUIList(BaseUI ui)
        {
            //如果已经存在了就让值归零
            if (_hideUiMap.TryGetValue(ui.name, out Content content))
                content.nurCount = 0;
            else
                //不存在就添加
                _hideUiMap.Add(ui.name, new Content() { baseUI = ui, nurCount = 0 });

            //检测所有UI的使用程度
            CheckAndDestoryBaseUI();
        }
        /// <summary>
        /// 检查是否已经算是最久未使用UI
        /// </summary>
        public void CheckAndDestoryBaseUI()
        {
            closedUIAfter.Clear();
            foreach (var item in _hideUiMap)
            {
                if (item.Value.nurCount == _closeUIThreshold)
                    closedUIAfter.Add(item.Key);
                else
                    item.Value.nurCount++;
            }
            foreach (var item in closedUIAfter)
            {
                tmpBaseui = _hideUiMap[item].baseUI;

                UIMgr.Instance.RemoveUIFromUIMap(tmpBaseui.name, tmpBaseui.uiType);
                _hideUiMap.Remove(item);
                Object.DestroyImmediate(tmpBaseui.gameObject);
            }
        }
    }
}
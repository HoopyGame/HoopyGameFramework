/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*��������δʹ�û����㷨��N�β�����UI�������
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
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
            /// ��ǰUI
            /// </summary>
            public BaseUI baseUI;
            /// <summary>
            /// ���û�в�����ǰUI��
            /// </summary>
            public int nurCount;
        }

        private Dictionary<string, Content> _hideUiMap;                           //�رյ���û�����ٵı�
        private readonly List<string> closedUIAfter;
        private readonly int _closeUIThreshold;                                   //���ٴ�δ�ٴδ򿪾�����
        private BaseUI tmpBaseui;


        public LeastResentlyUsedUtility(int closeUIThreshold)
        {
            _closeUIThreshold = closeUIThreshold;
            tmpBaseui = null;
            _hideUiMap = new Dictionary<string, Content>();
            closedUIAfter = new List<string>();
        }
        /// <summary>
        /// ��һ��UI�ӹرյ�δ����(������)���б����Ƴ�
        /// </summary>
        /// <param name="baseUI"></param>
        public void RemoveFromTotalHideUIList(string uiName)
        {
            _hideUiMap.Remove(uiName);
        }

        /// <summary>
        /// ��һ��UI����رյ�δ����(������)���б���
        /// </summary>
        /// <param name="baseUI"></param>
        public void AddToTotalHideUIList(BaseUI ui)
        {
            //����Ѿ������˾���ֵ����
            if (_hideUiMap.TryGetValue(ui.name, out Content content))
                content.nurCount = 0;
            else
                //�����ھ����
                _hideUiMap.Add(ui.name, new Content() { baseUI = ui, nurCount = 0 });

            //�������UI��ʹ�ó̶�
            CheckAndDestoryBaseUI();
        }
        /// <summary>
        /// ����Ƿ��Ѿ��������δʹ��UI
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
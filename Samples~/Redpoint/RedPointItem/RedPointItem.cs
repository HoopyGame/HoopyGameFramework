/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：红点
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;

namespace HoopyGame.RedPoint
{
    public class RedPointItem : MonoBehaviour
    {
        private RedPointModelBase _redPointModel;
        private RedPointNode _redPointNode;

        /// <summary>
        /// 这里使用策略模式来进行红点的判定
        /// 目前有的红点总类：纯红点，数字红点，每日红点
        /// </summary>
        /// <param name="redPoint">红点基类</param>
        public void OnInit(string redPointFullName, RedPointType redPointType,RedPointItem redPointItem,int redPointNumber)
        {
            _redPointModel = GeneratorRedPointItemFactory.CreateRedPointModel(redPointFullName, redPointType, redPointItem);
            //添加红点监听器
            _redPointNode = RedPointManager.Instance.AddListener(_redPointModel.Name, _redPointModel.OnRedPointValueChangeCallBack);
        }
    }
}


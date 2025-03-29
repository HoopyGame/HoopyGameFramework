/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：测试红点功能
│　创 建 人*：Hoopy
│　创建时间：2025-02-18 10:19:09
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using UnityEngine.UI;
using HoopyGame.RedPoint;

namespace HoopyGame
{
    public class TestRedPoint : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private string _nodeName;
        private RedPointItem _redPointItem;
        private Button _btn;

        [SerializeField]
        private int _redPointNubmer;
        #endregion

        #region SystemMethod
        #endregion
        #region MainMethod

        private void Awake()
        {
            _btn = gameObject.AddComponent<Button>();
            _redPointItem = GetComponent<RedPointItem>();
            gameObject.AddComponent<Image>();

            _redPointItem.OnInit(_nodeName, RedPointType.RedPointWithNumber, _redPointItem, _redPointNubmer);

            _btn.onClick.AddListener(() =>
            {
                _redPointNubmer++;
                RedPointManager.Instance.ChangeValue(_nodeName, _redPointNubmer);
            });
        }
        #endregion
        #region EventMethod

        #endregion
    }
}
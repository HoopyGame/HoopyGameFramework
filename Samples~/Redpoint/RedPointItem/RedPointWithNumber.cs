/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：数字红点
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using TMPro;
using UnityEngine;

namespace HoopyGame.RedPoint
{
    public class RedPointWithNumber : RedPointModelBase
    {
        private Transform _redPointWithNumber;

        private TextMeshProUGUI _redPointWithNumber_Txt;

        public RedPointWithNumber(string name, RedPointType redPointType, RedPointItem redpointItem) : base(name, redPointType)
        {
            _redPointWithNumber = redpointItem.transform.GetChild(0);
            Init();
        }
        private void Init()
        {
            _redPointWithNumber_Txt = _redPointWithNumber.GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        public override void OnRedPointValueChangeCallBack(int value)
        {
            _redPointWithNumber.gameObject.SetActive(value >= 1);
            _redPointWithNumber_Txt.enabled = value >= 2;
            _redPointWithNumber_Txt.SetText(value.ToString());
        }
    }
}

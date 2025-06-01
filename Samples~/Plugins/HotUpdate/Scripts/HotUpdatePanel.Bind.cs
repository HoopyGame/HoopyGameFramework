/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：HotUpdatePanel的数据组件绑定类
│　创 建 人*：Hoopy
│　创建时间：2025/5/6 16:51:47
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HoopyGame
{
	public partial class HotUpdatePanel
	{
		private Slider _progress_Sld;
		private TextMeshProUGUI _progress_Txt;
		private Image _shale_Img;
		private TextMeshProUGUI _status_Txt;
		private RectTransform _needUpdateTips_Trans;
		private TextMeshProUGUI _needUdpateContent_Txt;
		private Button _update_Btn;
		private Button _cancel_Btn;
		private RectTransform _errorTips_Trans;
		private TextMeshProUGUI _errorTipsContent_Txt;
		private Button _reset_Btn;

        private void InitComponent()
        {
 			_progress_Sld = transform.FindComponentFromChild<Slider>("Progress_Sld");
			_progress_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("Progress_Txt");
			_shale_Img = transform.FindComponentFromChild<Image>("Shale_Img");
			_status_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("Status_Txt");
			_needUpdateTips_Trans = transform.FindComponentFromChild<RectTransform>("NeedUpdateTips_Trans");
			_needUdpateContent_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("NeedUpdateTips_Trans/NeedUdpateContent_Txt");
			_update_Btn = transform.FindComponentFromChild<Button>("NeedUpdateTips_Trans/Update_Btn");
			_cancel_Btn = transform.FindComponentFromChild<Button>("NeedUpdateTips_Trans/Cancel_Btn");
			_errorTips_Trans = transform.FindComponentFromChild<RectTransform>("ErrorTips_Trans");
			_errorTipsContent_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("ErrorTips_Trans/ErrorTipsContent_Txt");
			_reset_Btn = transform.FindComponentFromChild<Button>("ErrorTips_Trans/Reset_Btn");

        }
	}
}
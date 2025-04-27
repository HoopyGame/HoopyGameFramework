/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：本地和网络的事件名称管理
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
namespace HoopyGame.Manager
{
    public class MsgStrMgr
    {
        public struct Local
        {
            /// <summary>
            /// 此行代码不要删，这个框架内切换场景用到
            /// </summary>
            public const string LoadSceneEvent = "LoadSceneEvent";

            #region 以下内容可自行配置删除
            public const string HideStudioTips = "HideStudioTips";
            public const string ShowStuidoTips = "ShowStudioTips";
            #endregion

        }
        public struct Net
        {

        }
    }
}
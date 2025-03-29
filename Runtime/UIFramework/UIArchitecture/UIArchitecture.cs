/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：
│　创 建 人*：Hoopy
│　创建时间：2025-03-01 15:35:14
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using HoopyGame.Core;

namespace HoopyGame.UIF
{
    public class UIArchitecture : HGArchitecture<UIArchitecture>
    {
        public override IOCContainer AppointParentIOC() => null;
        protected override void Init()
        {
            this.RegisterUtility(new NotUsedResentlyUtility(5));

        }
    }
}
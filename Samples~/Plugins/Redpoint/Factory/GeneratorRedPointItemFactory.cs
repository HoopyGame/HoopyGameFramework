/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：根据类型创建红点单元
│　创 建 人*：Hoopy
│　创建时间：2025-02-18 17:54:39
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
namespace HoopyGame.RedPoint
{
    public class GeneratorRedPointItemFactory
    {
        #region Variables
        #endregion
        #region SystemMethod
        #endregion
        #region MainMethod
        /// <summary>
        /// 创建一个红点模型
        /// </summary>
        /// <param name="redPointFullName"></param>
        /// <param name="redPointType"></param>
        /// <param name="redPointNumber"></param>
        /// <returns></returns>
        public static RedPointModelBase CreateRedPointModel(string redPointFullName,RedPointType redPointType,RedPointItem redPointItem)
        {
            return new RedPointWithNumber(redPointFullName, redPointType, redPointItem);
        }
        #endregion
        #region EventMethod
        #endregion
    }
}
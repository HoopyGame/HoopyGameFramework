/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：查找组件规则接口
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

namespace HoopyGame{
	public interface IBindRule
	{
        /// <summary>
        /// 判断物体是否可以绑定
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="filedNames">变量名称列表</param>
        /// <param name="componentTypeName">组件列表</param>
        /// <returns></returns>
        bool IsValid(Transform target, List<string> filedNames, List<string> componentTypeName);
    }
}
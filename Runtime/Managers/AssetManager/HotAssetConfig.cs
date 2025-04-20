/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：资源加载和更新的配置
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEngine;

namespace HoopyGame
{
    public class HotAssetConfig
    {
        public const string PackageName = "Main";                                //资源包名称

        public static string defaultHostServer = "http://127.0.0.1/CDN/";        //远端资源地址
        public static string fallbackHostServer = "http://127.0.0.1/CDN/";       //备用远端资源地址
        
        public static string GetDefaultHostServer
            => $"{GetPlatform}/{PackageName}";
        public static string GetFallbackHostServer
            => $"{GetPlatform}/{PackageName}";

        /// <summary>
        /// 获取平台
        /// </summary>
        public static string GetPlatform
        {
            get
            {
#if UNITY_ANDROID
                return "Android";
#elif UNITY_IOS
                return "IOS";
#elif UNITY_STANDALONE_OSX
                return "Mac";
#elif UNITY_STANDALONE_WIN
                return "Windows";
#elif UNITY_WEBGL
                return "WebGL";
#else
                return Application.platform.ToString();
#endif
            }
        }
    }
}
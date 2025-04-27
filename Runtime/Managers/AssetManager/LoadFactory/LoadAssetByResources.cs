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
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HoopyGame.Manager
{
    public class LoadAssetByResources : LoadAssetFactory
    {
        /// <summary>
        /// 同步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public override T LoadAssetSync<T>(string assetName, string packageName = null)
            => Resources.Load<T>(assetName);

        /// <summary>
        /// 异步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public override async UniTask<T> LoadAssetAsync<T>(string assetName, string packageName = null)
        {
            ResourceRequest request = Resources.LoadAsync<T>(assetName);
            await request;
            return request.asset as T;
        }
    }
}
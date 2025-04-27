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
using YooAsset;

namespace HoopyGame.Manager
{
    public class LoadAssetByYooAssets : LoadAssetFactory
    {

        /// <summary>
        /// 获取一个Package
        /// </summary>
        /// <param name="packageName">包名</param>
        /// <returns></returns>
        public ResourcePackage GetPacakge(string packageName)
        {
            if (string.IsNullOrEmpty(packageName)) packageName = HotAssetConfig.PackageName;
            return YooAssets.TryGetPackage(packageName);
        }
        /// <summary>
        /// 同步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public override T LoadAssetSync<T>(string assetName, string packageName = HotAssetConfig.PackageName)
        {
            AssetHandle assetHandle = GetPacakge(packageName).LoadAssetSync<T>(assetName);
            if (assetHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到该资源<{assetName}>");
            }
            return assetHandle.AssetObject as T;
        }
        /// <summary>
        /// 异步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public override async UniTask<T> LoadAssetAsync<T>(string assetName, string packageName = null)
        {
            AssetHandle assetHandle = GetPacakge(packageName).LoadAssetAsync<T>(assetName);
            await assetHandle;
            if (assetHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到类型为<{typeof(T)}>的资源<{assetName}>");
            }
            return assetHandle.AssetObject as T;
        }
    }
}
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：资源加载管理器
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
using UnityEngine.SceneManagement;
using YooAsset;

namespace HoopyGame
{
    /// <summary>
    /// 开启可寻址，可以模糊查找 如:HoopyGame.png可以写 HoopyGame|HoopyGame.png</summary>
    /// 没有开启可寻址要全路径查找
    /// </summary>
    public class AssetMgr
    {
        //Cache



        /// <summary>
        /// 加载一个资源
        /// <summary>
        /// <param name="location">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<T> LoadAssetAsync<T>(string location) where T : Object
        {
            AssetHandle ah = YooAssets.LoadAssetAsync<T>(location);
            await ah;
            if (ah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException("没有在包内找到该资源");
            }
            return ah.AssetObject as T;
        }
        /// <summary>
        /// 加载一个GameObject
        /// </summary>
        /// <param name="assetName">物体名字</param>
        /// <returns>返回该物体</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<GameObject> LoadAssetAsync(string assetName)
        {
            AssetHandle ah = YooAssets.LoadAssetAsync<GameObject>(assetName);
            await ah;
            if (ah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到{assetName}资源");
            }
            return ah.AssetObject as GameObject;
        }
        /// <summary>
        /// 加载一个热更脚本DLL（.byte-->TextAsset）
        /// textAsset.bytes 二进制数据
        /// textAsset.text 文本数据
        /// </summary>
        /// <param name="assetName">脚本名称</param>
        /// <returns>返回该脚本的TextAsset</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<TextAsset> LoadTextAsset(string textAssetName)
        {
            AssetHandle ah = YooAssets.LoadAssetAsync<TextAsset>(textAssetName);
            await ah;
            if (ah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到{textAssetName}资源");
            }
            return ah.AssetObject as TextAsset;
        }
        /// <summary>
        /// 资源包内所有对象加载（只要填写任何一个资源即可）
        /// 如配置表等，可以一开始就全部加载上
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="anyAssetName">任意一个资源名字</param>
        /// <returns>返回当前资源的集合</returns>
        /// <exception cref="MissingReferenceException"></exception> 
        public async UniTask<T[]> LoadAllAssetsAsync<T>(string anyAssetName) where T : Object
        {
            AllAssetsHandle allah = YooAssets.LoadAllAssetsAsync<T>(anyAssetName);
            await allah;
            if (allah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到{anyAssetName}资源");
            }
            return allah.AllAssetObjects as T[];
        }
        /// <summary>
        /// 加载T资源中的K资源
        /// </summary>
        /// <typeparam name="T">父资源类型</typeparam>
        /// <typeparam name="k">子资源类型</typeparam>
        /// <param name="parentAssetName">父资源名</param>
        /// <param name="subAssetName">子资源名</param>
        /// <returns></returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<k> LoadSubAssetAsync<T, k>(string parentAssetName, string subAssetName) where T : Object where k : Object
        {
            SubAssetsHandle subah = YooAssets.LoadSubAssetsAsync<T>(parentAssetName);
            await subah;
            if (subah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在{parentAssetName}包内找到{subAssetName}资源");
            }
            return subah.GetSubAssetObject<k>(subAssetName);
        }
        /// <summary>
        /// 从一个图集里加载某个图片
        /// </summary>
        /// <param name="parentTextureName">图片图集</param>
        /// <param name="subSpriteName">图片</param>
        /// <returns>返回Sprite</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<Sprite> LoadSubSpriteAssetAsync(string parentTextureName, string subSpriteName)
        {
            SubAssetsHandle subah = YooAssets.LoadSubAssetsAsync<Sprite>(parentTextureName);
            await subah;
            if (subah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在{parentTextureName}图集内找到{subSpriteName}图片");
            }
            return subah.GetSubAssetObject<Sprite>(subSpriteName);
        }
        /// <summary>
        /// 加载一个场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneMode"></param>
        /// <param name="localPhysicsMode"></param>
        /// <returns></returns>
        public async UniTask<Scene> LoadSceneAsync(string sceneName,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None,
            bool suspendLoad = false)
        {
            SceneHandle sceneh = YooAssets.LoadSceneAsync(sceneName, loadSceneMode, localPhysicsMode, suspendLoad);
            await sceneh;
            if (sceneh.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到场景{sceneName}");
            }
            return sceneh.SceneObject;
        }
        /// <summary>
        /// 获取当前Tag下所有资源的文件信息
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>标签下的所有文件信息</returns>
        public AssetInfo[] GetAssetInfosByTag(string tag)
        {
            return YooAssets.GetAssetInfos(tag);
        }
        /// <summary>
        /// 返回当前资源是否有更新
        /// </summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>是否有更新</returns>
        public bool IsNeedDownloadFromRemote(string assetName)
        {
            return YooAssets.IsNeedDownloadFromRemote(assetName);
        }

    }
}
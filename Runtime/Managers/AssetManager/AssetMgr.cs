/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：资源加载管理器 -->不使用YooAsset的话删
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
        AssetMgr()
        {
            DebugUtils.Print("初始化资源管理器...");
        }
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
        //Assets  --以下内容可以使用工厂模式配置
        /// <summary>
        /// 同步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public T LoadAssetSync<T>(string assetName, string packageName = HotAssetConfig.PackageName) where T : Object
        {
            AssetHandle assetHandle = GetPacakge(packageName).LoadAssetSync<T>(assetName);
            if (assetHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到该资源{assetName}");
            }
            return assetHandle.AssetObject as T;
        }
        /// <summary>
        /// 异步加载一个资源
        /// <summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>返回资源</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<T> LoadAssetAsync<T>(string assetName, string packageName = HotAssetConfig.PackageName) where T : Object
        {

            AssetHandle assetHandle = GetPacakge(packageName).LoadAssetAsync<T>(assetName);
            await assetHandle;
            if (assetHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在包内找到类型为<{typeof(T)}>的资源\"{assetName}\"");
            }
            return assetHandle.AssetObject as T;
        }
        /// <summary>
        /// 同步加载一个子资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="parentTextureName">父资源名</param>
        /// <param name="subSpriteName">子资源名</param>
        /// <param name="packageName">包名</param>
        /// <returns>T</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public T LoadSubAssetSync<T>(string parentTextureName, string subSpriteName, string packageName = HotAssetConfig.PackageName) where T:Object
        {
            SubAssetsHandle subAssetHandle = GetPacakge(packageName).LoadSubAssetsSync<T>(parentTextureName);
            if (subAssetHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在<{parentTextureName}>资源内找到<{subSpriteName}>子资源");
            }
            return subAssetHandle.GetSubAssetObject<T>(subSpriteName);
        }
        /// <summary>
        /// 异步加载一个子资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="parentTextureName">父资源名</param>
        /// <param name="subSpriteName">子资源名</param>
        /// <param name="packageName">包名</param>
        /// <returns>T</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<T> LoadSubAssetAsync<T>(string parentTextureName, string subSpriteName, string packageName = HotAssetConfig.PackageName)where T:Object
        {
            SubAssetsHandle subah = GetPacakge(packageName).LoadSubAssetsAsync<T>(parentTextureName);
            await subah;
            if (subah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有在<{parentTextureName}>资源内找到<{subSpriteName}>子资源");
            }
            return subah.GetSubAssetObject<T>(subSpriteName);
        }
        /// <summary>
        /// 同步资源包内所有对象加载（只要填写任何一个资源即可）
        /// 如配置表等，可以一开始就全部加载上
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="anyAssetName">任意一个资源名字</param>
        /// <returns>返回当前资源的集合</returns>
        /// <exception cref="MissingReferenceException"></exception> 
        public T[] LoadAllAssetsSync<T>(string anyAssetName, string packageName = HotAssetConfig.PackageName) where T : Object
        {
            AllAssetsHandle allah = GetPacakge(packageName).LoadAllAssetsSync<T>(anyAssetName);
            if (allah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到{anyAssetName}资源");
            }
            return allah.AllAssetObjects as T[];
        }
        /// <summary>
        /// 异步资源包内所有对象加载（只要填写任何一个资源即可）
        /// 如配置表等，可以一开始就全部加载上
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="anyAssetName">任意一个资源名字</param>
        /// <returns>返回当前资源的集合</returns>
        /// <exception cref="MissingReferenceException"></exception> 
        public async UniTask<T[]> LoadAllAssetsAsync<T>(string anyAssetName, string packageName = HotAssetConfig.PackageName) where T : Object
        {
            AllAssetsHandle allah = GetPacakge(packageName).LoadAllAssetsAsync<T>(anyAssetName);
            await allah;
            if (allah.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到{anyAssetName}资源");
            }
            return allah.AllAssetObjects as T[];
        }

        //Script
        /// <summary>
        /// 同步加载一个热更脚本DLL（.byte-->TextAsset）
        /// textAsset.bytes 二进制数据
        /// textAsset.text 文本数据
        /// </summary>
        /// <param name="textAssetName">资源名</param>
        /// <returns>TextAsset</returns>
        public TextAsset LoadTextAssetSync(string textAssetName, string packageName = HotAssetConfig.PackageName)
            => LoadAssetSync<TextAsset>(textAssetName, packageName);
        /// <summary>
        /// 异步加载一个热更脚本DLL（.byte-->TextAsset）
        /// textAsset.bytes 二进制数据
        /// textAsset.text 文本数据
        /// </summary>
        /// <param name="textAssetName">资源名</param>
        /// <returns>TextAsset</returns>
        public async UniTask<TextAsset> LoadTextAsset(string textAssetName, string packageName = HotAssetConfig.PackageName)
            => await LoadAssetAsync<TextAsset>(textAssetName, packageName);

        //GameObject
        /// <summary>
        /// 同步加载一个GameObject
        /// </summary>
        /// <param name="assetName">物体名字</param>
        /// <returns>返回该物体</returns>
        public GameObject LoadGameObjectSync(string assetName, string packageName = HotAssetConfig.PackageName)
            => LoadAssetSync<GameObject>(assetName, packageName);
        /// <summary>
        /// 异步加载一个GameObject
        /// </summary>
        /// <param name="assetName">物体名字</param>
        /// <returns>返回该物体</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<GameObject> LoadGameObjectAsync(string assetName, string packageName = HotAssetConfig.PackageName)
            => await LoadAssetAsync<GameObject>(assetName, packageName);

        //Sprite
        /// <summary>
        /// 同步从一个图集里加载某个图片
        /// </summary>
        /// <param name="parentTextureName">图片图集</param>
        /// <param name="subSpriteName">图片</param>
        /// <returns>返回Sprite</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public Sprite LoadSubSpriteAssetSync(string parentTextureName, string subSpriteName, string packageName = HotAssetConfig.PackageName)
            => LoadSubAssetSync<Sprite>(parentTextureName, subSpriteName, packageName);
        /// <summary>
        /// 异步从一个图集里加载某个图片
        /// </summary>
        /// <param name="parentTextureName">图片图集</param>
        /// <param name="subSpriteName">图片</param>
        /// <returns>返回Sprite</returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTask<Sprite> LoadSubSpriteAssetAsync(string parentTextureName, string subSpriteName, string packageName = HotAssetConfig.PackageName)
            => await LoadSubAssetAsync<Sprite>(parentTextureName, subSpriteName, packageName);

        //Scene
        /// <summary>
        /// 同步加载一个场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="isAdditive">是否叠加场景</param>
        /// <returns></returns>
        /// <exception cref="MissingReferenceException"></exception>
        public void LoadSceneSync(string sceneName, bool isAdditive, string packageName = HotAssetConfig.PackageName)
        {
            SceneHandle sceneHandle = GetPacakge(packageName).LoadSceneSync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            if (sceneHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到场景{sceneName}");
            }
            UnloadUnusedAssets();
        }
        /// <summary>
        /// 异步加载一个场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="isAdditive">是否叠加场景</param>
        /// <returns></returns>
        /// <exception cref="MissingReferenceException"></exception>
        public async UniTaskVoid LoadSceneAsync(string sceneName, bool isAdditive)
        {
            SceneHandle sceneHandle = YooAssets.LoadSceneAsync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            await sceneHandle;
            if (sceneHandle.Status != EOperationStatus.Succeed)
            {
                throw new MissingReferenceException($"没有找到场景{sceneName}");
            }
            UnloadUnusedAssets();
        }

        //help
        /// <summary>
        /// 获取当前Tag下所有资源的文件信息
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>标签下的所有文件信息</returns>
        public AssetInfo[] GetAssetInfosByTag(string tag, string packageName = HotAssetConfig.PackageName)
            =>GetPacakge(packageName).GetAssetInfos(tag);
        /// <summary>
        /// 返回当前资源是否有更新
        /// </summary>
        /// <param name="assetName">资源名字</param>
        /// <returns>是否有更新</returns>
        public bool IsNeedDownloadFromRemote(string assetName, string packageName = HotAssetConfig.PackageName)
            =>GetPacakge(packageName).IsNeedDownloadFromRemote(assetName);

        //Relese
        /// <summary>
        /// 卸载所有引用计数为零的资源包。
        /// 可以在切换场景之后调用资源释放方法或者写定时器间隔时间去释放。
        /// </summary>
        public void UnloadUnusedAssets(string packageName = HotAssetConfig.PackageName)
            => GetPacakge(packageName).UnloadUnusedAssetsAsync();
        /// <summary>
        /// 强制卸载所有资源包，该方法请在合适的时机调用
        /// 注意：Package在销毁的时候也会自动调用该方法
        /// </summary>
        public void ForceUnloadAllAssets(string packageName = HotAssetConfig.PackageName)
            =>GetPacakge(packageName).UnloadAllAssetsAsync();
        /// <summary>
        /// 尝试卸载指定的资源对象
        /// 注意：如果该资源还在被使用，该方法会无效
        /// </summary>
        public void TryUnloadUnusedAsset(string assetName, string packageName = HotAssetConfig.PackageName)
            =>GetPacakge(packageName).TryUnloadUnusedAsset(assetName);
    }
}
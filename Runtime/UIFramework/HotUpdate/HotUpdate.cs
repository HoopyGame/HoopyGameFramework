/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：资源热更新流程
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace HoopyGame.UIF
{
    public class HotUpdate : MonoBehaviour
    {
        //UI的控制
        private Slider _progress_slider;                        //进度条
        private TextMeshProUGUI _progress_text;                 //进度条文字
        private TextMeshProUGUI _status_text;                   //状态文字

        private Image _shale_img;                               //遮罩

        private Transform _needUpdateTips;                      //是否需要更新
        private TextMeshProUGUI _needUpdateContent_Text;        //是否需要更新文字
        private Button _update_Btn;                             //更新按钮
        private Button _cancel_Btn;                             //取消按钮
        //这里每次重试都要都要重新测试一下网络的连通性
        private Transform _errorTips;                           //错误信息提示
        private TextMeshProUGUI _errorTipsContent;              //错误提示内容
        private Button _reset_Btn;                              //重试

        //资源包配置相关
        [Header("资源配置相关")]
        [SerializeField]
        EPlayMode _playMode = EPlayMode.EditorSimulateMode;                         //资源加载模式
        [SerializeField]
        private string _packageName = "MainPackage";                                //资源包名称
        [SerializeField]
        private string packageVersion = "";                                         //资源版本
        [Header("远端资源地址配置")]
        [SerializeField]
        private string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";     //远端资源地址
        [SerializeField]
        private string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";    //备用远端资源地址
        [Header("资源包下载相关")]
        [SerializeField]
        private int downloadingMaxNum = 10;                                         //最大下载数量
        [SerializeField]
        private int failedTryAgain = 2;                                             //下载失败重试次数
        ResourceDownloaderOperation _downloaderOperation;                           //资源下载器操作类

        private ResourcePackage _mainPackage;
        private InitializationOperation _initializationOperation;

        private void Awake()
        {
            //获取组件
            OnInitComponent();
        }
        private void OnInitComponent()
        {
            _progress_slider = transform.FindComponentFromChild<Slider>("Progress_Slider");
            _progress_text = transform.FindTextMeshProUGUIFromChilds("Progress_Txt");
            _status_text = transform.FindTextMeshProUGUIFromChilds("Status_Txt");

            _shale_img = transform.FindImageFromChilds("Shale_Img");

            _needUpdateTips = transform.FindTransFromChild("NeedUpdateTips");
            _needUpdateContent_Text = _needUpdateTips.FindTextMeshProUGUIFromChilds("NeedUdpateContent_Txt");
            _update_Btn = _needUpdateTips.FindButtonFrommChilds("Update_Btn");
            _cancel_Btn = _needUpdateTips.FindButtonFrommChilds("Cancel_Btn");

            _errorTips = transform.FindTransFromChild("ErrorTips");
            _errorTipsContent = _errorTips.FindTextMeshProUGUIFromChilds("ErrorTipsContent_Txt");
            _reset_Btn = _errorTips.FindButtonFrommChilds("Reset_Btn");
        }
        void Start()
        {
            SetProgress(1);
            //1.初始化YooAssets
            YooAssets.Initialize();
            SetStatus("初始化完成");
            //2.创建资源包
            _mainPackage = YooAssets.TryGetPackage(_packageName);
            if (_mainPackage == null) _mainPackage = YooAssets.CreatePackage(_packageName);
            YooAssets.SetDefaultPackage(_mainPackage);
            BeginHotUpdate();
        }
        private void OnEnable()
        {
            _reset_Btn.OnRegister(() =>
            {
                HideErrorTips();
                CheckLoadStatus();
            });
        }
        private void OnDisable()
        {
            _reset_Btn.OnClear();
        }
        /// <summary>
        /// 开始热更新流程
        /// </summary>
        /// <returns></returns>
        private void BeginHotUpdate()
        {
            //3.设置资源包加载模式
            switch (_playMode)
            {
                case EPlayMode.EditorSimulateMode:
                    EditorSimulateMode().Forget();
                    break;
                case EPlayMode.OfflinePlayMode:
                    OfflinePlayMode().Forget();
                    break;
                case EPlayMode.HostPlayMode:
                    HostPlayMode().Forget();
                    break;
                case EPlayMode.WebPlayMode:
                    WebPlayMode().Forget();
                    break;
            }
        }

        /// <summary>
        /// 编辑器模拟模式
        /// </summary>
        private async UniTaskVoid EditorSimulateMode()
        {
            var buildResult = EditorSimulateModeHelper.SimulateBuild(_packageName);
            var packageRoot = buildResult.PackageRootDirectory;
            var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            var initParameters = new EditorSimulateModeParameters();
            initParameters.EditorFileSystemParameters = editorFileSystemParams;
            var initOperation = _mainPackage.InitializeAsync(initParameters);
            await initOperation;
            _initializationOperation = initOperation;
            CheckLoadStatus();
        }
        /// <summary>
        /// 单机离线模式
        /// </summary>
        private async UniTaskVoid OfflinePlayMode()
        {
            var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            var initParameters = new OfflinePlayModeParameters();
            initParameters.BuildinFileSystemParameters = buildinFileSystemParams;
            var initOperation = _mainPackage.InitializeAsync(initParameters);
            await initOperation;
            _initializationOperation = initOperation;
            CheckLoadStatus();
        }
        /// <summary>
        /// 在线模式
        /// </summary>
        private async UniTaskVoid HostPlayMode()
        {
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var cacheFileSystemParams = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

            var initParameters = new HostPlayModeParameters();
            initParameters.BuildinFileSystemParameters = buildinFileSystemParams;
            initParameters.CacheFileSystemParameters = cacheFileSystemParams;
            var initOperation = _mainPackage.InitializeAsync(initParameters);
            await initOperation;
            _initializationOperation = initOperation;
            CheckLoadStatus();
        }
        /// <summary>
        /// Web模式-->微信小游戏，抖音小游戏等WebGL平台
        /// </summary>
        private async UniTaskVoid WebPlayMode()
        {
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var webServerFileSystemParams = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
            var webRemoteFileSystemParams = FileSystemParameters.CreateDefaultWebRemoteFileSystemParameters(remoteServices); //支持跨域下载

            var initParameters = new WebPlayModeParameters();
            initParameters.WebServerFileSystemParameters = webServerFileSystemParams;
            initParameters.WebRemoteFileSystemParameters = webRemoteFileSystemParams;

            var initOperation = _mainPackage.InitializeAsync(initParameters);
            await initOperation;
            _initializationOperation = initOperation;
            CheckLoadStatus();
        }

        /// <summary>
        /// 检查资源加载状态
        /// </summary>
        /// <param name="operation"></param>
        private void CheckLoadStatus()
        {
            if (_initializationOperation.Status == EOperationStatus.Succeed)
            {
                GetAssetVersion().Forget();
                return;
            }
        }
        /// <summary>
        /// 获取资源版本号
        /// </summary>
        private async UniTaskVoid GetAssetVersion()
        {
            //4.资源加载完成后，获取资源包的版本号
            var requestOperation = _mainPackage.RequestPackageVersionAsync();
            await requestOperation;
            if (requestOperation.Status != EOperationStatus.Succeed)
            {
                //获取版本号失败
                ShowErrorTips($"获取资源版本号失败！", requestOperation.Error);
                return;
            }
            packageVersion = requestOperation.PackageVersion;
            SetStatus($"获取资源版本号成功：{packageVersion}");
            GetAssetManifestFile().Forget();
        }
        /// <summary>
        /// 获取更新清单
        /// </summary>
        /// <returns></returns>
        private async UniTaskVoid GetAssetManifestFile()
        {
            //5.获取文件清单列表
            var updateManifestOperation = _mainPackage.UpdatePackageManifestAsync(packageVersion);
            await updateManifestOperation;
            if (updateManifestOperation.Status != EOperationStatus.Succeed)
            {
                //更新文件清单失败
                ShowErrorTips($"更新文件清单失败！", updateManifestOperation.Error);
                return;
            }
            if (_playMode == EPlayMode.EditorSimulateMode || _playMode == EPlayMode.OfflinePlayMode)
            {
                //编辑器模拟模式下不需要下载资源
                SetStatus($"目前版本是最新版");
                await UniTask.WaitForSeconds(.5f);
                StartGame();
                return;
            }
            //检查网络连接 TODO

            //6.下载文件清单的内容
            _downloaderOperation = _mainPackage.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            //6.1没有需要下载的资源
            if (_downloaderOperation.TotalDownloadCount == 0)
            {
                //没有需要下载的资源
                DebugUtils.Print("无需要更新的资源");
                StartGame();
                return;
            }
            _downloaderOperation.DownloadUpdateCallback = OnDownloadUpdateFunction; //注册下载调用的回调
            //6.2有需要下载的资源 所有文件数量和文件大小
            int totalDownloadCount = _downloaderOperation.TotalDownloadCount;
            double totalDownloadBytes = _downloaderOperation.TotalDownloadBytes;

            //弹窗更新(检查是否强更新，非强更新可以取消)
        }

        /// <summary>
        /// 下载热更新资源
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownHotAssets()
        {
            //7.注册下载进度回调-->开始下载
            _downloaderOperation.BeginDownload(); //开始下载
            yield return _downloaderOperation; //等待下载完成
            //8.等待下载完成
            if (_downloaderOperation.Status != EOperationStatus.Succeed)
            {
                //下载失败
                Debug.LogError($"资源下载失败：{_downloaderOperation.Error}");
                yield break;
            }
            //下载成功 
            Debug.Log($"资源下载成功,开始清理不再使用的资源缓存-->{_downloaderOperation.BeginTime}");
            //9.清理资源包 清理文件系统未使用的缓存资源文件
            var clearCacheOperation = _mainPackage.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            yield return clearCacheOperation;
            if (clearCacheOperation.Status != EOperationStatus.Succeed)
            {
                //清理失败
                Debug.LogError($"文件清理失败：{clearCacheOperation.Error}");
            }
            else
            {
                //清理成功
                Debug.Log($"文件清理成功--> {clearCacheOperation.BeginTime}");
            }

            //开始游戏
            StartGame();
        }
        /// <summary>
        /// 下载进度回调函数
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnDownloadUpdateFunction(DownloadUpdateData data)
        {
            //TODO : 处理下载进度-->data.Progress data.CurrentDownloadBytes data.CurrentDownloadCount

        }
        //设置进度
        private void SetProgress(int value)
        {
            _progress_slider.value = value;
            _progress_text.text = $"{value * 100}%";
        }
        //设置状态
        private void SetStatus(string currentStatus)
        {
            _status_text.text = currentStatus;
            DebugUtils.Print(currentStatus);
        }
        //设置错误弹窗
        private void ShowErrorTips(string errorStr,string debug)
        {
            DebugUtils.Print(errorStr + "---->" + debug, DebugType.Error);
            _shale_img.DOFade(.8f, .2f);

            _errorTipsContent.text = errorStr;
            _errorTips.localScale = Vector3.zero;
            _errorTips.gameObject.SetActive(true);
            _errorTips.DOScale(Vector3.one, .56f)
                .SetEase(Ease.OutBack);
        }
        public void HideErrorTips()
        {
            _shale_img.DOFade(0f, .2f);
            _errorTips.DOScale(Vector3.zero, .56f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _errorTips.gameObject.SetActive(false);
                });
        }
        private void SetUpdateTips(string updateTips)
        {

        }

        //进入游戏逻辑
        private void StartGame()
        {
            
        }
        //IEnumerator LoadAsset()
        //{
        //    AssetHandle handle = _mainPackage.LoadAssetAsync<GameObject>("Assets/Arts/Prefabs/Cube.prefab");
        //    yield return handle;
        //    GameObject go = handle.InstantiateSync();
        //    Debug.Log($"Prefab name is {go.name}");

        //    AssetHandle handle2 = _mainPackage.LoadAssetAsync<Texture2D>("Assets/Arts/Prefabs/Black.png");
        //    yield return handle2;
        //    Texture2D texture = handle2.AssetObject as Texture2D;
        //    Debug.Log(texture.name);
        //    i.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        //    i.SetNativeSize();
        //}
        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string _defaultHostServer;
            private readonly string _fallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                _defaultHostServer = defaultHostServer;
                _fallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{_defaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{_fallbackHostServer}/{fileName}";
            }
        }
    }

}

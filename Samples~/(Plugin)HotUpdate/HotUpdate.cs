/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*����Դ�ȸ�������
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
*/
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YooAsset;

namespace HoopyGame.UIF
{
    public class HotUpdate : MonoBehaviour
    {
        //UI�Ŀ���
        private Slider _progress_slider;                        //������
        private TextMeshProUGUI _progress_text;                 //����������
        private TextMeshProUGUI _status_text;                   //״̬����

        private Image _shale_img;                               //����

        private Transform _needUpdateTips;                      //�Ƿ���Ҫ����
        private TextMeshProUGUI _needUpdateContent_Text;        //�Ƿ���Ҫ��������
        private Button _update_Btn;                             //���°�ť
        private Button _cancel_Btn;                             //ȡ����ť
        //����ÿ�����Զ�Ҫ��Ҫ���²���һ���������ͨ��
        private Transform _errorTips;                           //������Ϣ��ʾ
        private TextMeshProUGUI _errorTipsContent;              //������ʾ����
        private Button _reset_Btn;                              //����

        //��Դ���������
        [Header("��Դ�������")]
        [SerializeField]
        EPlayMode _playMode = EPlayMode.EditorSimulateMode;                         //��Դ����ģʽ
        [SerializeField]
        private string _packageName = "MainPackage";                                //��Դ������
        [SerializeField]
        private string _cloudPackageVersion = "";                                    //�ƶ���Դ�汾
        private string _localPackageVersion = "0.0.0";                               //������Դ�汾
        [Header("Զ����Դ��ַ����")]
        [SerializeField]
        private string defaultHostServer = "http://127.0.0.1/CDN/Android/v1.0";     //Զ����Դ��ַ
        [SerializeField]
        private string fallbackHostServer = "http://127.0.0.1/CDN/Android/v1.0";    //����Զ����Դ��ַ
        [Header("��Դ���������")]
        [SerializeField]
        private int downloadingMaxNum = 10;                                         //�����������
        [SerializeField]
        private int failedTryAgain = 2;                                             //����ʧ�����Դ���
        ResourceDownloaderOperation _downloaderOperation;                           //��Դ������������

        private ResourcePackage _mainPackage;
        private InitializationOperation _initializationOperation;

        private void Awake()
        {
            //��ȡ���
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
            //1.��ʼ��YooAssets
            YooAssets.Initialize();
            SetStatus("�����Դ����O.o");
            //2.������Դ��
            _mainPackage = YooAssets.TryGetPackage(_packageName);
            if (_mainPackage == null) _mainPackage = YooAssets.CreatePackage(_packageName);
            else _localPackageVersion = _mainPackage.GetPackageVersion();

            DebugUtils.Print($"������Դ���汾:{_localPackageVersion}");
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
            _update_Btn.OnRegister(OnUpdateBtnClick);
            _update_Btn.OnRegister(HideUpdateTips);
            _cancel_Btn.OnRegister(OnCancelBtnClick);
        }
        private void OnDisable()
        {
            _reset_Btn.OnClear();
            _update_Btn.OnUnRegister(OnUpdateBtnClick);
            _update_Btn.OnUnRegister(HideUpdateTips);
            _cancel_Btn.OnUnRegister(OnCancelBtnClick);
        }
        /// <summary>
        /// ��ʼ�ȸ�������
        /// </summary>
        /// <returns></returns>
        private void BeginHotUpdate()
        {
            //3.������Դ������ģʽ
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
        /// �༭��ģ��ģʽ
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
        /// ��������ģʽ
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
        /// ����ģʽ
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
        /// Webģʽ-->΢��С��Ϸ������С��Ϸ��WebGLƽ̨
        /// </summary>
        private async UniTaskVoid WebPlayMode()
        {
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var webServerFileSystemParams = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
            var webRemoteFileSystemParams = FileSystemParameters.CreateDefaultWebRemoteFileSystemParameters(remoteServices); //֧�ֿ�������

            var initParameters = new WebPlayModeParameters();
            initParameters.WebServerFileSystemParameters = webServerFileSystemParams;
            initParameters.WebRemoteFileSystemParameters = webRemoteFileSystemParams;

            var initOperation = _mainPackage.InitializeAsync(initParameters);
            await initOperation;
            _initializationOperation = initOperation;
            CheckLoadStatus();
        }

        /// <summary>
        /// �����Դ����״̬
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
        /// ��ȡ��Դ�汾��
        /// </summary>
        private async UniTaskVoid GetAssetVersion()
        {
            //4.��Դ������ɺ󣬻�ȡ��Դ���İ汾��
            var requestOperation = _mainPackage.RequestPackageVersionAsync();
            await requestOperation;
            if (requestOperation.Status != EOperationStatus.Succeed)
            {
                //��ȡ�汾��ʧ��
                ShowErrorTips($"��ȡ��Դ�汾��ʧ�ܣ�", requestOperation.Error);
                return;
            }
            _cloudPackageVersion = requestOperation.PackageVersion;
            //SetStatus($"��ȡ��Դ�汾�ųɹ���{packageVersion}");
            DebugUtils.Print($"��ȡ��Դ�汾�ųɹ���{_cloudPackageVersion}");
            GetAssetManifestFile().Forget();
        }
        /// <summary>
        /// ��ȡ�����嵥
        /// </summary>
        /// <returns></returns>
        private async UniTaskVoid GetAssetManifestFile()
        {

            if (_playMode == EPlayMode.EditorSimulateMode || _playMode == EPlayMode.OfflinePlayMode)
            {
                //5.��ȡ�ļ��嵥�б�
                var offlineManifestOperation = _mainPackage.UpdatePackageManifestAsync(_cloudPackageVersion);
                await offlineManifestOperation;
                if (offlineManifestOperation.Status != EOperationStatus.Succeed)
                {
                    //�����ļ��嵥ʧ��
                    ShowErrorTips($"�����ļ��嵥ʧ�ܣ�", offlineManifestOperation.Error);
                    return;
                }
                //�༭��ģ��ģʽ�²���Ҫ������Դ
                SetStatus($"��Դ�����µ�o.O");
                await UniTask.WaitForSeconds(.5f);
                StartGame();
                return;
            }
            //�����������
            var hasInternet = await HasInternetAccess();
            if(!hasInternet)
            {
                //û������
                ShowErrorTips("������������!", "û������......");
                return;
            }
            //5.��ȡ�ļ��嵥�б�
            var updateManifestOperation = _mainPackage.UpdatePackageManifestAsync(_cloudPackageVersion);
            await updateManifestOperation;
            if (updateManifestOperation.Status != EOperationStatus.Succeed)
            {
                //�����ļ��嵥ʧ��
                ShowErrorTips($"�����ļ��嵥ʧ�ܣ�", updateManifestOperation.Error);
                return;
            }
            //6.�����ļ��嵥������
            _downloaderOperation = _mainPackage.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            //6.1û����Ҫ���ص���Դ
            if (_downloaderOperation.TotalDownloadCount == 0)
            {
                //û����Ҫ���ص���Դ
                SetStatus($"Ŀǰ��Դ�����µ�O.o");
                StartGame();
                return;
            }
            _downloaderOperation.DownloadUpdateCallback = OnDownloadUpdateFunction; //ע�����ص��õĻص�
            //6.2����Ҫ���ص���Դ �����ļ��������ļ���С
            int totalDownloadCount = _downloaderOperation.TotalDownloadCount;
            double totalDownloadBytes = _downloaderOperation.TotalDownloadBytes;

            //��������(����Ƿ�ǿ���£���ǿ���¿���ȡ��)  TODO-->ĿǰҪ��ǿ�Ƹ���
            ShowUpdateTips($"��鵽��Դ���£��Ƿ���и��£�\n��Դ��С��{totalDownloadBytes / 1048576:F2}MB"
                , $"��鵽��Դ���£�������{totalDownloadCount},��С��{totalDownloadBytes / 1048576:F2}MB");

        }
        //��������Ƿ�ͨ��
        public async UniTask<bool> HasInternetAccess()
        {
            // ʹ�ò��᷵��404�ı�׼API�˵�
            string[] testUrls = new string[]
            {
                "https://www.baidu.com",                // �ٶ�
                "https://www.google.com",               // �ȸ践�ؿ����ݵĵ�ַ
            };

            foreach (var url in testUrls)
            {
                try
                {
                    using UnityWebRequest request = UnityWebRequest.Head(url);
                    request.timeout = 3; // 3�볬ʱ
                    var operation = request.SendWebRequest();

                    while (!operation.isDone)
                        await UniTask.Yield();

                    if (request.result == UnityWebRequest.Result.Success ||
                       request.responseCode == 204) // 204 No Content
                    {
                        return true;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return false;
        }
        //������°�ť
        private void OnUpdateBtnClick()
        {
            //������°�ť
            SetStatus($"����������Դo.O");
            //��ʼ������Դ
            DownHotAssets().Forget();
        }
        //���ȡ�����°�ť
        private void OnCancelBtnClick()
        {
            //�ж��Ƿ���ǿ�Ƹ��£�TODO
            Application.Quit();
        }

        /// <summary>
        /// �����ȸ�����Դ
        /// </summary>
        /// <returns></returns>
        private async UniTaskVoid DownHotAssets()
        {
            //7.ע�����ؽ��Ȼص�-->��ʼ����
            _downloaderOperation.BeginDownload(); //��ʼ����
            await _downloaderOperation; //�ȴ��������
            //8.�ȴ��������
            if (_downloaderOperation.Status != EOperationStatus.Succeed)
            {
                //����ʧ��
                ShowErrorTips("��Դ����ʧ��!", $"��Դ����ʧ�ܣ�{_downloaderOperation.Error}");
                return;
            }
            //���سɹ� 
            DebugUtils.Print($"��Դ���سɹ�,��ʼ������ʹ�õ���Դ����-->{_downloaderOperation.BeginTime}");
            //9.������Դ�� �����ļ�ϵͳδʹ�õĻ�����Դ�ļ�
            var clearCacheOperation = _mainPackage.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            await clearCacheOperation;
            if (clearCacheOperation.Status != EOperationStatus.Succeed)
                //����ʧ��
                DebugUtils.Print($"�ļ�����ʧ�ܣ�{clearCacheOperation.Error}", DebugType.Error);
            else
                //����ɹ�
                DebugUtils.Print($"�ļ�����ɹ�--> {clearCacheOperation.BeginTime}");

            //��ʼ��Ϸ
            await UniTask.WaitForSeconds(.5f);
            SetStatus("��Դ�������");
            StartGame();
        }
        /// <summary>
        /// ���ؽ��Ȼص�����
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnDownloadUpdateFunction(DownloadUpdateData data)
        {
            //TODO : �������ؽ���-->data.Progress data.CurrentDownloadBytes data.CurrentDownloadCount
            _progress_slider.value = data.Progress;
            _progress_text.text = $"{data.Progress * 100:F2}%";
        }
        //���ý���
        private void SetProgress(int value)
        {
            _progress_slider.value = value;
            _progress_text.text = $"{value * 100}%";
        }
        //����״̬
        private void SetStatus(string currentStatus)
        {
            _status_text.text = currentStatus;
            DebugUtils.Print(currentStatus);
        }
        //���ô��󵯴�
        private void ShowErrorTips(string errorStr, string debug)
        {
            DebugUtils.Print(debug, DebugType.Error);
            _shale_img.DOFade(.8f, .25f);

            _errorTipsContent.text = errorStr;
            _errorTips.localScale = Vector3.zero;
            _errorTips.gameObject.SetActive(true);
            _errorTips.DOScale(Vector3.one, .4f)
                .SetEase(Ease.OutBack);
        }
        public void HideErrorTips()
        {
            _shale_img.DOFade(0f, .25f);
            _errorTips.DOScale(Vector3.zero, .3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _errorTips.gameObject.SetActive(false);
                });
        }
        //���ø�����ʾ
        private void ShowUpdateTips(string errorStr, string debug)
        {
            DebugUtils.Print(debug);
            _shale_img.DOFade(.8f, .25f);

            _needUpdateContent_Text.text = errorStr;
            _needUpdateTips.localScale = Vector3.zero;
            _needUpdateTips.gameObject.SetActive(true);
            _needUpdateTips.DOScale(Vector3.one, .4f)
                .SetEase(Ease.OutBack);
        }
        public void HideUpdateTips()
        {
            _shale_img.DOFade(0f, .25f);
            _needUpdateTips.DOScale(Vector3.zero, .3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _needUpdateTips.gameObject.SetActive(false);
                });
        }

        //������Ϸ�߼�
        private void StartGame()
        {

        }
        /// <summary>
        /// Զ����Դ��ַ��ѯ������
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

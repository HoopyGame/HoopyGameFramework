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
using TMPro;
using UnityEngine;
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
        private string packageVersion = "";                                         //��Դ�汾
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
            SetStatus("��ʼ�����");
            //2.������Դ��
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
            packageVersion = requestOperation.PackageVersion;
            SetStatus($"��ȡ��Դ�汾�ųɹ���{packageVersion}");
            GetAssetManifestFile().Forget();
        }
        /// <summary>
        /// ��ȡ�����嵥
        /// </summary>
        /// <returns></returns>
        private async UniTaskVoid GetAssetManifestFile()
        {
            //5.��ȡ�ļ��嵥�б�
            var updateManifestOperation = _mainPackage.UpdatePackageManifestAsync(packageVersion);
            await updateManifestOperation;
            if (updateManifestOperation.Status != EOperationStatus.Succeed)
            {
                //�����ļ��嵥ʧ��
                ShowErrorTips($"�����ļ��嵥ʧ�ܣ�", updateManifestOperation.Error);
                return;
            }
            if (_playMode == EPlayMode.EditorSimulateMode || _playMode == EPlayMode.OfflinePlayMode)
            {
                //�༭��ģ��ģʽ�²���Ҫ������Դ
                SetStatus($"Ŀǰ�汾�����°�");
                await UniTask.WaitForSeconds(.5f);
                StartGame();
                return;
            }
            //����������� TODO

            //6.�����ļ��嵥������
            _downloaderOperation = _mainPackage.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            //6.1û����Ҫ���ص���Դ
            if (_downloaderOperation.TotalDownloadCount == 0)
            {
                //û����Ҫ���ص���Դ
                DebugUtils.Print("����Ҫ���µ���Դ");
                StartGame();
                return;
            }
            _downloaderOperation.DownloadUpdateCallback = OnDownloadUpdateFunction; //ע�����ص��õĻص�
            //6.2����Ҫ���ص���Դ �����ļ��������ļ���С
            int totalDownloadCount = _downloaderOperation.TotalDownloadCount;
            double totalDownloadBytes = _downloaderOperation.TotalDownloadBytes;

            //��������(����Ƿ�ǿ���£���ǿ���¿���ȡ��)
        }

        /// <summary>
        /// �����ȸ�����Դ
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownHotAssets()
        {
            //7.ע�����ؽ��Ȼص�-->��ʼ����
            _downloaderOperation.BeginDownload(); //��ʼ����
            yield return _downloaderOperation; //�ȴ��������
            //8.�ȴ��������
            if (_downloaderOperation.Status != EOperationStatus.Succeed)
            {
                //����ʧ��
                Debug.LogError($"��Դ����ʧ�ܣ�{_downloaderOperation.Error}");
                yield break;
            }
            //���سɹ� 
            Debug.Log($"��Դ���سɹ�,��ʼ������ʹ�õ���Դ����-->{_downloaderOperation.BeginTime}");
            //9.������Դ�� �����ļ�ϵͳδʹ�õĻ�����Դ�ļ�
            var clearCacheOperation = _mainPackage.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            yield return clearCacheOperation;
            if (clearCacheOperation.Status != EOperationStatus.Succeed)
            {
                //����ʧ��
                Debug.LogError($"�ļ�����ʧ�ܣ�{clearCacheOperation.Error}");
            }
            else
            {
                //����ɹ�
                Debug.Log($"�ļ�����ɹ�--> {clearCacheOperation.BeginTime}");
            }

            //��ʼ��Ϸ
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

        //������Ϸ�߼�
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

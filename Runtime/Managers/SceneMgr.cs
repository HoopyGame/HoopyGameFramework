/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：场景加载管理器
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using HoopyGame.UIF;
using VContainer.Unity;
using HoopyGame;

public class SceneMgr : SingleBaseMono<SceneMgr>
{
    private Transform m_loadSceneUIPanel;
    private TextMeshProUGUI m_loadSceneProgress_Txt;
    private Slider m_loadSceenProgress_Sld;

    private float _loadProgress;


    /// <summary>
    /// 下一个要加载的场景
    /// </summary>
    public string NextLoadSceneName { get; private set; }

    public override void OnInit()
    {
        base.OnInit();
        InitComponent();
    }
    private void InitComponent()
    {
        m_loadSceneUIPanel = UIMgr.Instance.UIRoot.FindTransFromChild("LoadingPanel");
        m_loadSceenProgress_Sld = m_loadSceneUIPanel.FindComponentFromChild<Slider>("LoadSceenProgress_Sld");
        m_loadSceneProgress_Txt = m_loadSceneUIPanel.FindComponentFromChild<TextMeshProUGUI>("LoadSceenProgress_Txt");
    }
    
    /// <summary>
    /// 场景同步加载，会阻塞
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="fun">回调</param>
    /// <param name="loadSceneMode">场景加载模式</param>
    public void LoadScene(string sceneName, UnityAction fun = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        //场景同步加载
        SceneManager.LoadScene(sceneName, loadSceneMode);
        LifetimeScopeHelper.Instance.Get<EventMgr>(typeof(GameLifetimeScope))
            .TriggerEvent(MsgStrMgr.Local.LoadSceneEvent);
        //加载完成过后 才会去执行fun
        fun?.Invoke();
    }
    public void LoadScene(int sceneindex, UnityAction fun = null, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        //场景同步加载
        SceneManager.LoadScene(sceneindex, loadSceneMode);
        LifetimeScopeHelper.Instance.Get<EventMgr>(typeof(GameLifetimeScope))
            .TriggerEvent(MsgStrMgr.Local.LoadSceneEvent);
        //加载完成过后 才会去执行fun
        fun?.Invoke();
    }

    /// <summary>
    /// 异步加载场景（需要LoadScene支持）
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="allowSceneActivation">是否允许直接加载</param>
    /// <param name="loadSceneMode"></param>
    public void LoadSceneAsyncByLoading(string sceneName)
    {
        NextLoadSceneName = sceneName;
        try
        {
            SceneManager.LoadScene("Loading");
        }
        catch
        {
            throw new MissingReferenceException("若没有Loading场景请不要使用LoadSceneAsyncByLoading");
        }
    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">场景名字</param>
    /// <param name="loadSceneMode">场景加载模式</param>
    public AsyncOperation LoadSceneAsync(string sceneName, bool allowSceneActivation, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        operation.allowSceneActivation = allowSceneActivation;
        return operation;
    }
    public AsyncOperation LoadSceneAsync(int sceneIndex, bool allowSceneActivation, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        operation.allowSceneActivation = allowSceneActivation;
        return operation;
    }

    /// <summary>
    /// 从持久化场景加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneAsyncByNameFromPermenetScene(string sceneName)
    {
        if (m_loadSceneUIPanel)
            LoadProgress(sceneName);
        else
        {
            DebugUtils.Print("当前或许不是从持久化场景内进行的场景加载，跳过进度，直接加载");
            LoadSceneAsync(sceneName, true);
        }
    }
    /// <summary>
    /// 从持久化场景加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneAsyncByNameFromPermenetScene(int sceneIndex)
    {
        if (m_loadSceneUIPanel)
            LoadProgress(sceneIndex);
        else
        {
            DebugUtils.Print("当前或许不是从持久化场景内进行的场景加载，跳过进度，直接加载");
            LoadSceneAsync(sceneIndex, true);
        }
    }
    public void UnloadSceneByName(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    public void UnloadSceneByIndex(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    public void ResetGame(bool clearPersistentData = true)
    {
        if (clearPersistentData) { PlayerPrefs.DeleteAll(); }
        SceneManager.LoadScene(0);
    }
    private async void LoadProgress(string sceneName)
    {
        m_loadSceneUIPanel.gameObject.SetActive(true);

        AsyncOperation operation = LoadSceneAsync(sceneName, false, LoadSceneMode.Additive);
        operation.completed += (AsyncOperation asyncOp) => { m_loadSceneUIPanel.gameObject.SetActive(false); };
        _loadProgress = 0;
        while (!operation.isDone)
        {
            _loadProgress = Mathf.Lerp(_loadProgress, operation.progress / .9f, Time.deltaTime);
            m_loadSceneProgress_Txt.text = (int)(_loadProgress * 100) + "%";
            m_loadSceenProgress_Sld.value = _loadProgress;
            if (_loadProgress >= .99f)
            {
                m_loadSceneProgress_Txt.text = "100%";
                m_loadSceenProgress_Sld.value = 1;
                operation.allowSceneActivation = true;
                return;
            }
            await UniTask.Yield();
        }
    }
    private async void LoadProgress(int sceneIndex)
    {
        m_loadSceneUIPanel.gameObject.SetActive(true);

        AsyncOperation operation = LoadSceneAsync(sceneIndex, false, LoadSceneMode.Additive);
        operation.completed += (AsyncOperation asyncOp) => { m_loadSceneUIPanel.gameObject.SetActive(false); };
        _loadProgress = 0;
        while (!operation.isDone)
        {
            _loadProgress = Mathf.Lerp(_loadProgress, operation.progress / .9f, Time.deltaTime);
            m_loadSceneProgress_Txt.text = (int)(_loadProgress * 100) + "%";
            m_loadSceenProgress_Sld.value = _loadProgress;
            if (_loadProgress >= .99f)
            {
                m_loadSceneProgress_Txt.text = "100%";
                m_loadSceenProgress_Sld.value = 1;
                operation.allowSceneActivation = true;
                return;
            }
            await UniTask.Yield();
        }
    }

}

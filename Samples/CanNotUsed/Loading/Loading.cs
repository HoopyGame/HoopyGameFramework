/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：场景加载的过渡场景
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Loading : MonoBehaviour
{
    private TextMeshProUGUI m_loadSceneProgress_Txt;
    private Slider m_loadSceenProgress_Sld;

    private float _loadProgress;

    private void Awake()
    {
        m_loadSceenProgress_Sld = transform.FindComponentFromChild<Slider>("LoadSceenProgress_Sld");
        m_loadSceneProgress_Txt = transform.FindComponentFromChild<TextMeshProUGUI>("LoadSceenProgress_Txt");
    }
    private async void Start()
    {

        AsyncOperation operation = SceneMgr.Instance.LoadSceneAsync(SceneMgr.Instance.NextLoadSceneName, false, LoadSceneMode.Single);
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

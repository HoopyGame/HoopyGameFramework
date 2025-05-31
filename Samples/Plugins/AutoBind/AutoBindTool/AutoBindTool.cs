using System.Net.Mime;
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：根据后缀自动绑定UI,可自定义绑定规则
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HoopyGame
{
    [ExecuteInEditMode]
    public class AutoBindTool : MonoBehaviour
    {
#if UNITY_EDITOR
        private const string _defaultSavePath = "/Scripts/UI/";

        [SerializeField]
        private string _className = "XXX_UI(输入...)";
        [SerializeField]
        private string _nameSpace = "HoopyGame";
        [SerializeField]
        private string _codeSavePath = _defaultSavePath;
        public string ClassName { get => _className; }
        public string NameSpace { get => _nameSpace; }
        public string CodeSavePath { get =>  _codeSavePath; }

        public IBindRule bindRule;

        public List<BindData> bindData = new List<BindData>();

        [InitializeOnLoadMethod]
        public static void OnInit()
        {
            if (!Directory.Exists(Application.dataPath + _defaultSavePath))
                Directory.CreateDirectory(Application.dataPath + _defaultSavePath);
        }
        private void OnEnable()
        {
            //避免打包的时候打包此脚本
            hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
        }

        
#endif
    }
    [Serializable]
    public class BindData
    {
        public string name;
        public Component bindComponent;

        public BindData() { }
        public BindData(string name, Component bindComponent)
        {
            this.name = name; this.bindComponent = bindComponent;
        }
    }
}
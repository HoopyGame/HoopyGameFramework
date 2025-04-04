/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudioStudio
│　描   述*：导入常用的工具包
│　创 建 人*：Hoopy
│　创建时间：2025-03-05 09:39:51
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HoopyGame.Editor
{
    public class ImportPackageEditor : EditorWindow
	{
        // 定义页面枚举
        private enum Page
        {
            Packages,
            Documents,
            Samples
        }
        // 当前选中的页面
        private Page currentPage = Page.Packages;

        private Vector3 currentPos;

        private static GitHubPackagesList _gitHubPackageList;

        private string _matchString;

        private List<Infomation> _infomations;

        private const string filePathInAsset = 
            "Assets/com.hoopygame.hoopygameframework/Editor/EditorWindows/GitHubPackage/GitHubPackagesList.asset";
        private const string filePathInPackage = 
            "Packages/com.hoopygame.hoopygameframework/Editor/EditorWindows/GitHubPackage/GitHubPackagesList.asset";

        [MenuItem("DBug/Tools/ImportGitHubPackage",priority =12)]
		public static void Open()
		{
            GetWindow<ImportPackageEditor>("GitHubPackage");

            string filePath = File.Exists(filePathInAsset) ? filePathInAsset : filePathInPackage;

            _gitHubPackageList = AssetDatabase.LoadAssetAtPath<GitHubPackagesList>(filePath);
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                this.Close(); // 关闭窗口
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentPage == Page.Packages, "Packages", EditorStyles.toolbarButton))
            {
                currentPage = Page.Packages;
            }
            if (GUILayout.Toggle(currentPage == Page.Documents, "Documents", EditorStyles.toolbarButton))
            {
                currentPage = Page.Documents;
            }
            if (GUILayout.Toggle(currentPage == Page.Samples, "Samples", EditorStyles.toolbarButton))
            {
                currentPage = Page.Samples;
            }
            GUILayout.EndHorizontal();

            switch (currentPage)
            {
                case Page.Packages:
                    DrawPage("PackageList", _gitHubPackageList.PackageList);
                    break;
                case Page.Documents:
                    DrawPage("DocumentList", _gitHubPackageList.Documents);
                    break;
                case Page.Samples:
                    DrawPage("SampleList", _gitHubPackageList.Samples);
                    break;
            }

        }

        private void DrawPage(string title,List<Infomation> packageList)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, new GUIStyle()
            {
                fontSize = 20,
                normal = new GUIStyleState() { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
            });
            _matchString = GUILayout.TextField(_matchString);
            if (!string.IsNullOrEmpty(_matchString))
                _infomations = packageList.Where(item => (item.name + item.infomation + item.URL).ToLower().Contains(_matchString.ToLower())).ToList();
            else
                _infomations = packageList;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            LoadURL(_infomations);
        }


        #region Obsolete
        private void DrawPage1()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("PackageList", new GUIStyle()
            {
                fontSize = 20,
                normal = new GUIStyleState() { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
            });
            _matchString = GUILayout.TextField(_matchString);
            if (!string.IsNullOrEmpty(_matchString))
                _infomations = _gitHubPackageList.PackageList.Where(item => (item.name + item.infomation + item.URL).ToLower().Contains(_matchString.ToLower())).ToList();
            else
                _infomations = _gitHubPackageList.PackageList;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            LoadURL(_infomations);
        }
        private void DrawPage2()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("DocumentList", new GUIStyle()
            {
                fontSize = 20,
                normal = new GUIStyleState() { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
            });
            _matchString = GUILayout.TextField(_matchString);
            if (!string.IsNullOrEmpty(_matchString))
                _infomations = _gitHubPackageList.Documents.Where(item => (item.name + item.infomation + item.URL).ToLower().Contains(_matchString.ToLower())).ToList();
            else
                _infomations = _gitHubPackageList.Documents;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            LoadURL(_infomations);
        }
        private void DrawPage3()
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("SampleList", new GUIStyle()
            {
                fontSize = 20,
                normal = new GUIStyleState() { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
            });
            _matchString = GUILayout.TextField(_matchString);
            if (!string.IsNullOrEmpty(_matchString))
                _infomations = _gitHubPackageList.Samples.Where(item => (item.name + item.infomation + item.URL).ToLower().Contains(_matchString.ToLower())).ToList();
            else
                _infomations = _gitHubPackageList.Samples;
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            LoadURL(_infomations);
        }

        #endregion
        public void LoadURL(List<Infomation> list)
        {
            currentPos = GUILayout.BeginScrollView(currentPos);
            foreach (var package in list)
            {
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                GUILayout.Label(package.name ,new GUIStyle()
                {
                    fontSize = 15,
                    normal = new GUIStyleState() { textColor = Color.white}
                }) ;
                GUILayout.Label(package.infomation,new GUIStyle()
                {
                    fontSize = 12,
                    normal = new GUIStyleState() { textColor = Color.green}
                });
                GUILayout.Space(10);
                GUILayout.EndVertical();

                if (GUILayout.Button("打开"))
                {
                    Application.OpenURL(package.URL);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
    }
}
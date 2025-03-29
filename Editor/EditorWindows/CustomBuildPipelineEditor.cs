/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：自动化打包脚本
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
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HoopyGame.Editor
{
    public class CustomBuildPipelineEditor:EditorWindow
    {
        private static string pcName = PlayerSettings.productName + ".exe";
        private static string androidName = PlayerSettings.productName + ".apk";

        private static string currentName;

        private static string _buildPath;
        public int seelctNumber;

        public enum BuildPipelineType
        {
            Windows,
            Android,
            WebGL,
            IOS
        }
        private BuildPipelineType _type;
        private ScriptingImplementation _implementation;

        private static EditorWindow _window;

        [MenuItem("DBug/Tools/BuildPipelineEditor",priority = 1)]
        public static void Open()
        {
           EditorWindow.GetWindow<CustomBuildPipelineEditor>("CustomBuildPipeline");
            
        }

        private void OnGUI()
        {
            _window = this;

            EditorGUILayout.Space(5);
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("选择打包平台:");
            _type = (BuildPipelineType)EditorGUILayout.EnumPopup(_type);
            _implementation = (ScriptingImplementation)EditorGUILayout.EnumPopup(_implementation);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("打包路径: " + _buildPath);
            if (GUILayout.Button("选择路径"))
            {
                _buildPath = EditorUtility.OpenFolderPanel("请选择DLL所在文件夹！", Application.dataPath, null);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("开始打包"))
            {
                switch (_type)
                {
                    case BuildPipelineType.Windows:
                        BuildPC((int)_implementation);
                        break;
                    case BuildPipelineType.Android:
                        BuildAndroid((int)_implementation);
                        break;
                    case BuildPipelineType.WebGL:
                        BuildWebGL((int)_implementation);
                        break;
                    case BuildPipelineType.IOS:
                        break;
                }
            }
        }

        public static void BuildWebGL(int scriptingImplementation=0)
        {
            currentName = "";
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.WebGL, (ScriptingImplementation)scriptingImplementation);
            BuildOpt(_buildPath??=CreatePath("WebGLOutPut"), BuildTarget.WebGL, BuildTargetGroup.WebGL);
        }
        public static void BuildAndroid(int scriptingImplementation = 0)
        {
            currentName = "/" + androidName;
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Android, (ScriptingImplementation)scriptingImplementation);
            BuildOpt(_buildPath ??= CreatePath("AndroidOutPut"), BuildTarget.Android, BuildTargetGroup.Android);
        }
        public static void BuildPC(int scriptingImplementation = 0)
        {
            currentName = "/" + pcName;
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone, (ScriptingImplementation)scriptingImplementation);
            BuildOpt(_buildPath ??= CreatePath("AndroidOutPut"), BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
        }

        private static string CreatePath(string buildTarget)
        {
            var buildPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + PlayerSettings.productName + buildTarget;
            if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);
            return buildPath;
        }
        private static void BuildOpt(string BuildPath, BuildTarget buildTarget, BuildTargetGroup targetGroup, BuildOptions buildOptions = BuildOptions.None)
        {
            var selectScenes = GetAllBuildSceens();
            if (selectScenes.Length == 0) throw new System.Exception("没有选择任何要打包的场景");

            BuildPlayerOptions buildPlayerOptions = new()
            {
                scenes = selectScenes,
                locationPathName = BuildPath + currentName,
                target = buildTarget,
                options = buildOptions,
                targetGroup = targetGroup
            };
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build Succeeded: " + summary.totalSize/1024 + " MB");
                _window.ShowNotification(new GUIContent("Build Succeeded !"));
                //System.Diagnostics.Process.Start(BuildPath);
            }
            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build Failed");
                _window.ShowNotification(new GUIContent("Build Failed!"));
            }
        }
        public static string[] GetAllBuildSceens()
        {
            List<string> scenePaths = new();

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                EditorBuildSettingsScene editorBuildSettingsScene = EditorBuildSettings.scenes[i];
                if (editorBuildSettingsScene.enabled)
                {
                    scenePaths.Add(editorBuildSettingsScene.path);
                }
            }
            foreach (var item in scenePaths)
            {
                Debug.Log("当前打包出的Scene：" + item);
            }
            return scenePaths.ToArray();
        }
    }
}

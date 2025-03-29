/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

[InitializeOnLoad]
public static class MultiDependencyInstaller
{
    // 配置你的所有依赖项
    private static readonly Dictionary<string, string> RequiredDependencies = new Dictionary<string, string>
    {
        {"com.tuyoogame.yooasset" , "2.3.6"}
    };

    // 配置需要的注册表
    private static readonly List<RegistryConfig> RequiredRegistries = new List<RegistryConfig>
    {
        new RegistryConfig
        {
            name = "package.openupm.com",
            url = "https://package.openupm.com",
            scopes = new[] {"com.tuyoogame.yooasset" }
        },
    };

    private struct RegistryConfig
    {
        public string name;
        public string url;
        public string[] scopes;
    }

    static MultiDependencyInstaller()
    {
        EditorApplication.delayCall += CheckAllDependencies;
    }

    [MenuItem("DBug/ImportDependences")]
    public static void CheckAllDependencies()
    {
        if (SessionState.GetBool("AllDependenciesChecked", false))
            return;

        SessionState.SetBool("AllDependenciesChecked", true);

        // 1. 先确保所有需要的注册表都已添加
        AddRequiredRegistries();

        // 2. 检查并添加所有缺失的包
        CheckAndInstallMissingPackages();
    }

    private static void AddRequiredRegistries()
    {
        string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
        string manifestContent = File.ReadAllText(manifestPath);
        bool manifestChanged = false;

        // 检查并添加每个需要的注册表
        foreach (var registry in RequiredRegistries)
        {
            if (!manifestContent.Contains($"\"{registry.name}\""))
            {
                string registryJson = $@"
        {{
            ""name"": ""{registry.name}"",
            ""url"": ""{registry.url}"",
            ""scopes"": [""{string.Join("\", \"", registry.scopes)}""]
        }}";

                if (!manifestContent.Contains("\"scopedRegistries\": ["))
                {
                    // 如果还没有任何注册表，添加整个注册表数组
                    manifestContent = manifestContent.Replace(
                        @"""dependencies"": {",
                        @"""scopedRegistries"": [" + registryJson + @"],
    ""dependencies"": {");
                }
                else
                {
                    // 如果已有其他注册表，追加到数组中
                    int insertPos = manifestContent.IndexOf("\"scopedRegistries\": [") + 21;
                    manifestContent = manifestContent.Insert(insertPos, registryJson + ",");
                }

                manifestChanged = true;
            }
        }

        if (manifestChanged)
        {
            File.WriteAllText(manifestPath, manifestContent, Encoding.UTF8);
            Client.Resolve();
        }
    }

    private static void CheckAndInstallMissingPackages()
    {
        ListRequest listRequest = Client.List();
        EditorApplication.update += OnListProgress;

        void OnListProgress()
        {
            if (!listRequest.IsCompleted) return;
            EditorApplication.update -= OnListProgress;

            if (listRequest.Status == StatusCode.Success)
            {
                var installedPackages = new HashSet<string>();
                foreach (var pkg in listRequest.Result)
                {
                    installedPackages.Add(pkg.name);
                }

                // 批量安装所有缺失的包
                foreach (var dependency in RequiredDependencies)
                {
                    if (!installedPackages.Contains(dependency.Key))
                    {
                        InstallPackage(dependency.Key, dependency.Value);
                    }
                }
            }
        }
    }

    private static void InstallPackage(string packageId, string version)
    {
        AddRequest addRequest = Client.Add($"{packageId}@{version}");
        EditorUtility.DisplayProgressBar("安装依赖", $"正在安装 {packageId}...", 0.5f);

        EditorApplication.update += OnAddProgress;

        void OnAddProgress()
        {
            if (!addRequest.IsCompleted) return;
            EditorApplication.update -= OnAddProgress;
            EditorUtility.ClearProgressBar();

            if (addRequest.Status != StatusCode.Success)
            {
                Debug.LogError($"安装 {packageId} 失败: {addRequest.Error.message}");
            }
        }
    }
}
#endif
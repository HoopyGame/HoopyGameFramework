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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HoopyGame
{
    public static class ScopedRegistryHelper
    {
        //ScopedRegistry newRegistry = new ScopedRegistry
        //{
        //    name = "package.openupm.com",
        //    url = "https://package.openupm.com",
        //    scopes = new string[]
        //    {
        //        "com.tuyoogame.yooasset",
        //        "jp.hadashikick.vcontainer"
        //    }
        //};
        [MenuItem("DBug/EditorTools/LoadCustomScopeRegistry")]
        public static void AddScopedRegistry()
        {
            // 配置要添加的注册表
            var registry = new ScopedRegistryConfig
            {
                name = "package.openupm.com",
                url = "https://package.openupm.com",
                scopes = new string[]
                {
                    "com.tuyoogame.yooasset",
                    "jp.hadashikick.vcontainer"
                }
            };

            AddScopedRegistryToManifest(registry);
        }

        private static void AddScopedRegistryToManifest(ScopedRegistryConfig registry)
        {
            string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            if (!File.Exists(manifestPath))
            {
                Debug.LogError("manifest.json not found!");
                return;
            }

            string originalJson = File.ReadAllText(manifestPath);

            // 1. 检查是否已存在相同注册表
            if (Regex.IsMatch(originalJson, $@"""name""\s*:\s*""{registry.name}""") ||
                Regex.IsMatch(originalJson, $@"""url""\s*:\s*""{registry.url}"""))
            {
                Debug.LogWarning($"Scoped Registry '{registry.name}' already exists!");
                return;
            }

            // 2. 构建新注册表的 JSON 片段
            string registryJson = $@"
    {{
        ""name"": ""{registry.name}"",
        ""url"": ""{registry.url}"",
        ""scopes"": [""{string.Join("\", \"", registry.scopes)}""]
    }}";

            // 3. 定位插入位置
            if (originalJson.Contains("\"scopedRegistries\""))
            {
                // 已有 scopedRegistries 的情况：插入到数组末尾
                originalJson = Regex.Replace(
                    originalJson,
                    @"(""scopedRegistries""\s*:\s*\[)(.*?)(\])",
                    match => $"{match.Groups[1].Value}{match.Groups[2].Value}{(match.Groups[2].Value.Trim().Length > 0 ? "," : "")}{registryJson}\n        {match.Groups[3].Value}",
                    RegexOptions.Singleline
                );
            }
            else
            {
                // 没有 scopedRegistries 的情况：在 dependencies 前插入
                originalJson = Regex.Replace(
                    originalJson,
                    @"(\{)(\s*""dependencies"")",
                    match => $"{match.Groups[1].Value}\n    \"scopedRegistries\": [{registryJson}],{match.Groups[2].Value}",
                    RegexOptions.Singleline
                );
            }

            // 4. 写入文件（保留所有原有格式）
            File.WriteAllText(manifestPath, originalJson);
            Debug.Log($"Added Scoped Registry: {registry.name}");

            // 5. 强制刷新包管理器
            UnityEditor.PackageManager.Client.Resolve();
        }

        [System.Serializable]
        private class ScopedRegistryConfig
        {
            public string name;
            public string url;
            public string[] scopes;
        }

    }
}
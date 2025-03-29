/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：用于资源导入初始化
│　创  建  人：DD
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修  改  人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.IO;
using UnityEditor;
using UnityEditor.Presets;

namespace HoopyGame.Editor
{
    public class PresetImportPerFolder : AssetPostprocessor
    {
        void OnPreprocessAsset()
        {
            // 确保我们在第一次导入资源时应用预设。
            if (assetImporter.importSettingsMissing)
            {
                // 获取当前导入的资源文件夹。
                var path = Path.GetDirectoryName(assetPath);
                while (!string.IsNullOrEmpty(path))
                {
                    // 查找此文件夹中的所有预设资源。
                    var presetGuids = AssetDatabase.FindAssets("t:Preset", new[] { path });
                    foreach (var presetGuid in presetGuids)
                    {
                        // 确保不是在子文件夹中测试预设。
                        string presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                        if (Path.GetDirectoryName(presetPath) == path)
                        {
                            //加载预设，然后尝试将其应用于导入器。
                            var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                            if (preset.ApplyTo(assetImporter))
                                return;
                        }
                    }

                    //在父文件夹中重试。
                    path = Path.GetDirectoryName(path);
                }
            }
        }
    }
}
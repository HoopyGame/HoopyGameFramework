/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：拷贝物体的路径
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using UnityEditor;
using UnityEngine;

namespace HoopyGame.Editor
{
    public class CopyObjectPathTool
    {
        [MenuItem("GameObject/Copy Name(Include\"\")", priority = 20)]
        public static void CopySelectionAssetName()
        {
            if (Selection.activeGameObject)
                EditorGUIUtility.systemCopyBuffer = $"\"{Selection.activeGameObject.name}\"";

        }
        [MenuItem("GameObject/Copy Full Path", priority = 20)]
        public static void CopySelectionAssetPath()
        {
            string path = string.Empty;
            //有选择到资源
            if (Selection.activeGameObject != null)
            {
                path = GetPath(Selection.activeGameObject.transform);
            }
            EditorGUIUtility.systemCopyBuffer = path;
        }

        private static string GetPath(Transform t)
        {
            if (!t.parent) return t.name;
            return GetPath(t.parent) + "/" + t.name;
        }
    }
}

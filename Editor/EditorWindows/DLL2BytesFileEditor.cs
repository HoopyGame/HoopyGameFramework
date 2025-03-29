/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：将DLL转和字节文件互转（代码热更用）
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
using UnityEditor;
using UnityEngine;

namespace HoopyGame.Editor
{
    public class DLL2BytesFileEditor : EditorWindow
    {
        private static string dllPath;
        private static string outputPath;

        private static bool isChangePDBFile;

        /// <summary>
        /// 选择文件夹将文件夹内的所有dll
        /// </summary>
        [MenuItem("DBug/Tools/Dll2BytesPanel",priority = 11)]
        public static void SelectDirDLL2Bytes()
        {
            EditorWindow.GetWindow(typeof(DLL2BytesFileEditor), true, "DLL转换Bytes工具");
        }

        private void OnGUI()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                this.Close(); // 关闭窗口
            }
            isChangePDBFile = GUILayout.Toggle(isChangePDBFile, "是否转换PDB文件？");

            GUI.skin.label.fontSize = 15;
            if (GUILayout.Button("请选择DLL文件夹路径", GUILayout.MinHeight(30)))
            {
                dllPath = EditorUtility.OpenFolderPanel("请选择DLL所在文件夹！", Application.dataPath, null);
                outputPath = dllPath;
            }
            GUILayout.Label("dllPath:\n" + dllPath);
            GUILayout.Space(15);
            if (GUILayout.Button("请选择输出路径", GUILayout.MinHeight(30)))
            {
                outputPath = EditorUtility.OpenFolderPanel("请选择输出文件夹！", Application.dataPath, null);
            }
            GUILayout.Label("outputPath:\n" + outputPath);
            GUILayout.Space(15);
            if (GUILayout.Button("开始转换", GUILayout.MinHeight(30)))
            {
                if (string.IsNullOrEmpty(dllPath) || string.IsNullOrEmpty(outputPath))
                {
                    //EditorUtility.DisplayDialog("错误", "dll文件或输出路径为空", "确定");
                    ShowNotification(new GUIContent("错误:dll文件或输出路径为空"));
                    return;
                }
                DirectoryInfo directoryInfo = new (dllPath);
                List<FileInfo> dllFiles = new();
                List<FileInfo> pdbFiles = new ();

                foreach (var file in directoryInfo.GetFiles())
                {
                    if (file.Extension.Equals(".dll")) dllFiles.Add(file);
                    else if (file.Extension.Equals(".pdb")) pdbFiles.Add(file);
                }
                //开始转换dll文件
                foreach (FileInfo file in dllFiles)
                {
                    string savePath = $"{outputPath}/{Path.GetFileNameWithoutExtension(file.Name)}_dll_bytes.bytes";
                    Bytes2File(savePath, File2Bytes(file));
                }
                //开始转换pdb文件
                if (isChangePDBFile)
                {
                    foreach (FileInfo file in pdbFiles)
                    {
                        string savePath = $"{outputPath}/{Path.GetFileNameWithoutExtension(file.Name)}_pdb_bytes.bytes";
                        Bytes2File(savePath, File2Bytes(file));
                    }

                }

                //EditorUtility.DisplayDialog("", "转换完成", "确定");
                ShowNotification(new GUIContent("转换完成!"));
                AssetDatabase.Refresh();
            }
        }
        /// <summary>
        /// 文件转换Bytes
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <returns></returns>
        public static byte[] File2Bytes(FileInfo file)
        {
            return File.ReadAllBytes(file.FullName);
        }

        /// <summary>
        /// Bytes转文件
        /// </summary>
        /// <param name="filePath">要存放的路径</param>
        /// <param name="bytes">bytes</param>
        public static void Bytes2File(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }
    }
}
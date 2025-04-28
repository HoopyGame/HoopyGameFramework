/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：创建脚本模板
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace HoopyGame.Editor
{
    public class GeneratorCustomScriptFile
    {
        //com.hg.hoopygameframework\Editor\ScriptCreater
        //Assets\com.hoopygame.hoopygameframework\Editor\ScriptCreater\Template
        private const string assetScriptTempatePath =
            "Assets/com.hoopygame.hoopygameframework/Editor/ScriptCreater/Template/";
        private const string packageScriptTempatePath =
            "Packages/HoopyGame Framework/Editor/ScriptCreater/Template/";

        [MenuItem("Assets/Create/Noraml C#", false, 60)]
        public static void GeneratorCSharp()
        {
            Generator("NormalTemplate");
        }
        [MenuItem("Assets/Create/Core/HGArchitecture C#", false, 59)]
        public static void GeneratorHGArchitectureCSharp()
        {
            Generator("HGArchitecture");
        }
        [MenuItem("Assets/Create/Core/IController", false, 59)]
        public static void GeneratorIControllerCSharp()
        {
            Generator("Controller");
        }
        [MenuItem("Assets/Create/Core/AbstractModel", false, 59)]
        public static void GeneratorAbstractModelCSharp()
        {
            Generator("AbstractModel");
        }
        [MenuItem("Assets/Create/Core/AbstractCommand", false, 59)]
        public static void GeneratorAbstractCommandCSharp()
        {
            Generator("AbstractCommand");
        }

        [MenuItem("Assets/Create/Core/AbstractSystem", false, 59)]
        public static void GeneratorAbstractSystemCSharp()
        {
            Generator("AbstractSystem");
        }
        [MenuItem("Assets/Create/Core/IUtility", false, 59)]
        public static void GeneratorIUtilityCSharp()
        {
            Generator("Utility");
        }

        public static void Generator(string csharpFileName)
        {
            string filename = csharpFileName + ".cs.txt";

            string filePath = File.Exists(assetScriptTempatePath + filename) ? assetScriptTempatePath + filename : packageScriptTempatePath + filename;
            
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                   ScriptableObject.CreateInstance<CreateEventCSScriptAsset>(),
                   GetSelectPathOrFallback() + "/" + csharpFileName + ".cs", EditorGUIUtility.FindTexture("cs Script Icon"),
                  filePath);
        }



        public static string GetSelectPathOrFallback()
        {
            string path = "Assets";
            //遍历选中的资源以获得路径  
            //Selection.GetFiltered是过滤选择文件或文件夹下的物体，assets表示只返回选择对象本身  
            foreach (
                UnityEngine.Object obj in
                Selection.GetFiltered(typeof(UnityEngine.Object),
                SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }
            return path;
        }

        //要创建模板文件必须继承EndNameEditAction，重写action方法  
        class CreateEventCSScriptAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                //创建资源  
                UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(obj);//高亮显示资源  
            }

            internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
            {
                //获取要创建资源的绝对路径  
                string fullPath = Path.GetFullPath(pathName);

                //读取本地的模板文件  
                StreamReader streamReader = new StreamReader(resourceFile);
                string text = streamReader.ReadToEnd();
                streamReader.Close();

                //获取文件名，不含扩展名  
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

                //将模板类中的类名替换成你创建的文件名  
                text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
                text = Regex.Replace(text, "#NowTime#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //写入配置文件  
                bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记  
                bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常  
                bool append = false;
                UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
                StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
                streamWriter.Write(text);
                streamWriter.Close();

                //刷新资源管理器  
                AssetDatabase.ImportAsset(pathName);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
            }
        }
    }
}
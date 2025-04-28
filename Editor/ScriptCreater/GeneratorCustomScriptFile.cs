/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
������������������������������������������������������������������������������������������������
����Copyright(C) 2025 by HoopyGameStudio
������   ��*�������ű�ģ��
������ �� ��*��Hoopy
��������ʱ�䣺2025-01-01 00:00:00
������������������������������������������������������������������������������������������������
������������������������������������������������������������������������������������������������
������ �� �ˣ�
�����޸�������
������������������������������������������������������������������������������������������������
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
            //����ѡ�е���Դ�Ի��·��  
            //Selection.GetFiltered�ǹ���ѡ���ļ����ļ����µ����壬assets��ʾֻ����ѡ�������  
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

        //Ҫ����ģ���ļ�����̳�EndNameEditAction����дaction����  
        class CreateEventCSScriptAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                //������Դ  
                UnityEngine.Object obj = CreateScriptAssetFromTemplate(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(obj);//������ʾ��Դ  
            }

            internal static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string resourceFile)
            {
                //��ȡҪ������Դ�ľ���·��  
                string fullPath = Path.GetFullPath(pathName);

                //��ȡ���ص�ģ���ļ�  
                StreamReader streamReader = new StreamReader(resourceFile);
                string text = streamReader.ReadToEnd();
                streamReader.Close();

                //��ȡ�ļ�����������չ��  
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);

                //��ģ�����е������滻���㴴�����ļ���  
                text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithoutExtension);
                text = Regex.Replace(text, "#NowTime#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //д�������ļ�  
                bool encoderShouldEmitUTF8Identifier = true; //����ָ���Ƿ��ṩ Unicode �ֽ�˳����  
                bool throwOnInvalidBytes = false;//�Ƿ��ڼ�⵽��Ч�ı���ʱ�����쳣  
                bool append = false;
                UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
                StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
                streamWriter.Write(text);
                streamWriter.Close();

                //ˢ����Դ������  
                AssetDatabase.ImportAsset(pathName);
                AssetDatabase.Refresh();
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
            }
        }
    }
}
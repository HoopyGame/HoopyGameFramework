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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HoopyGame.Editor
{
    [CustomEditor(typeof(AutoBindTool))]
    public class AutoBindInspector : UnityEditor.Editor
    {
        private AutoBindTool _autoBindTool;

        private SerializedProperty _bindDatas;

        private SerializedProperty _nameSpace;
        private SerializedProperty _className;
        private SerializedProperty _codeSavePath;

        //程序中的程序集
        private List<string> _assemblyNames = new List<string> { "Assembly-CSharp", "HoopyGameRuntime" };
        private string[] _bindRuleNames;
        private List<IBindRule> _bindRules = new List<IBindRule>();
        private List<string> tempFiledNames = new List<string>();
        private List<string> tempComponentTypeNames = new List<string>();
        private string _bindRule;
        private int _bindRuleIndex;

        private void OnEnable()
        {
            _autoBindTool = (AutoBindTool)target;
            _bindDatas = serializedObject.FindProperty("bindData");

            _bindRuleNames = GetBindRules(_assemblyNames);
            for (int i = 0; i < _bindRuleNames.Length; i++)
            {
                _bindRules.Add(CreateRuleInstance(_bindRuleNames[i], _assemblyNames));
            }

            _nameSpace = serializedObject.FindProperty("_nameSpace");
            _className = serializedObject.FindProperty("_className");
            _codeSavePath = serializedObject.FindProperty("_codeSavePath");

            serializedObject.ApplyModifiedProperties();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawRuleSelect();

            DrawSetting();

            DrawFuncButton();

            DrawBindDatas();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 绘制绑定规则选择框
        /// </summary>
        private void DrawRuleSelect()
        {
            _bindRule = _bindRuleNames[0];

            if (_autoBindTool.bindRule != null)
            {
                _bindRule = _autoBindTool.bindRule.GetType().Name;

                for (int i = 0; i < _bindRuleNames.Length; i++)
                {
                    if (_bindRule == _bindRuleNames[i]) _bindRuleIndex = i;
                }
            }
            else
            {
                IBindRule rule = CreateRuleInstance(_bindRule, _assemblyNames);
                _autoBindTool.bindRule = rule;
            }

            foreach (var item in Selection.gameObjects)
            {
                AutoBindTool bindTool = item.GetComponent<AutoBindTool>();
                if (bindTool.bindRule == null)
                {
                    IBindRule rule = CreateRuleInstance(_bindRule, _assemblyNames);
                    bindTool.bindRule = rule;
                }
            }

            int selectedIndex = EditorGUILayout.Popup("BindRule", _bindRuleIndex, _bindRuleNames);

            if (selectedIndex != _bindRuleIndex)
            {
                _bindRuleIndex = selectedIndex;
                _bindRule = _bindRuleNames[_bindRuleIndex];
                IBindRule rule = CreateRuleInstance(_bindRule, _assemblyNames);
                _autoBindTool.bindRule = rule;
            }
        }
        /// <summary>
        /// 绘制基础设置项
        /// </summary>
        private void DrawSetting()
        {
            EditorGUILayout.BeginHorizontal();

            _nameSpace.stringValue = EditorGUILayout.TextField("命名空间: ", _nameSpace.stringValue);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            _className.stringValue = EditorGUILayout.TextField("类名: ", _className.stringValue);

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.LabelField("代码保存路径:");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_codeSavePath.stringValue);


            if (GUILayout.Button("选择保存路径"))
            {
                string temp = _codeSavePath.stringValue;
                _codeSavePath.stringValue = EditorUtility.OpenFolderPanel("选择代码保存路径", Application.dataPath + _codeSavePath.stringValue, "");
                if (string.IsNullOrEmpty(_codeSavePath.stringValue))
                {
                    _codeSavePath.stringValue = temp;
                }
            }

            EditorGUILayout.EndHorizontal();
        }
        /// <summary>
        /// 绘制功能按钮
        /// </summary>
        private void DrawFuncButton()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("GetBindComponent"))
            {
                BindComponent();
            }

            if (GUILayout.Button("GeneratorBindCode"))
            {
                CreateBindCode();
            }

            if (GUILayout.Button("AddBindCodeScript"))
            {
                AddTargetClass();
            }

            EditorGUILayout.EndHorizontal();
        }
        /// <summary>
        /// 绑定组件
        /// </summary>
        private void BindComponent()
        {
            _bindDatas.ClearArray();
            Transform[] childs = _autoBindTool.GetComponentsInChildren<Transform>(true);

            foreach (var child in childs)
            {
                tempFiledNames.Clear();
                tempComponentTypeNames.Clear();
                if (_autoBindTool.bindRule.IsValid(child, tempFiledNames, tempComponentTypeNames))
                {
                    for (int i = 0; i < tempFiledNames.Count; i++)
                    {
                        Component com = child.GetComponent(tempComponentTypeNames[i]);
                        if (com == null)
                        {
                            Debug.LogError($"{child.name}上不存在{tempComponentTypeNames[i]}组件");
                        }
                        else
                        {
                            AddBindData(tempFiledNames[i], com);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 添加绑定数据
        /// </summary>
        /// <param name="name">变量名</param>
        /// <param name="bindCom">组件</param>
        private void AddBindData(string name, Component bindCom)
        {
            int index = _bindDatas.arraySize;
            _bindDatas.InsertArrayElementAtIndex(index);
            SerializedProperty element = _bindDatas.GetArrayElementAtIndex(index);

            element.FindPropertyRelative("name").stringValue = name;
            element.FindPropertyRelative("bindComponent").objectReferenceValue = bindCom;
        }
        /// <summary>
        /// 绘制绑定列表
        /// </summary>
        private void DrawBindDatas()
        {
            int deleteIndex = -1;

            EditorGUILayout.BeginVertical();
            SerializedProperty property;

            for (int i = 0; i < _bindDatas.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(25));
                property = _bindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("name");
                property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
                property = _bindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("bindComponent");
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Component), true);

                if (GUILayout.Button("－"))
                {
                    deleteIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("＋"))
            {
                AddBindData("", null);
            }

            if (deleteIndex != -1)
            {
                _bindDatas.DeleteArrayElementAtIndex(deleteIndex);
            }

            EditorGUILayout.EndVertical();
        }
        /// <summary>
        /// 生成绑定代码
        /// </summary>
        private void CreateBindCode()
        {
            GameObject obj = _autoBindTool.gameObject;

            string className = _autoBindTool.ClassName;
            string codePath = _autoBindTool.CodeSavePath;

            if (!Directory.Exists(codePath))
            {
                Debug.LogError($"{obj.name}的绑定代码保存路径{codePath}无效");
                return;
            }

            //脚本%%替换
            var tmpBindScript = bindScript;
            tmpBindScript = tmpBindScript.Replace("#NAMESPACE#", _autoBindTool.NameSpace);
            tmpBindScript = tmpBindScript.Replace("#SCRIPTNAME#", className);
            tmpBindScript = tmpBindScript.Replace("#TIME#", DateTime.Now.ToString());
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            foreach (var data in _autoBindTool.bindData)
            {
                sb1.AppendLine($"\t\tprivate {data.bindComponent.GetType().Name} _{char.ToLower(data.name[0]) + data.name.Substring(1)};");
                sb2.AppendLine($"\t\t\t_{char.ToLower(data.name[0]) + data.name.Substring(1)} = transform.FindComponentFromChild<{data.bindComponent.GetType().Name}>(\"{GetComponentPath(data.bindComponent.transform)}\");");
            }
            tmpBindScript = tmpBindScript.Replace("#COMPONENTDATAFIELD#", sb1.ToString());
            tmpBindScript = tmpBindScript.Replace("#COMPONENTDATAS#", sb2.ToString());
            sb1.Clear();
            sb2.Clear();

            using (StreamWriter sw = new StreamWriter($"{codePath}/{className}.Bind.cs"))
            {
                sw.Write(tmpBindScript);
                sw.Close();
            }
            //非绑定脚本的%%替换规则
            var tmpNotBindScript = notBindScript;
            tmpNotBindScript = tmpNotBindScript.Replace("#NAMESPACE#", _autoBindTool.NameSpace);
            tmpNotBindScript = tmpNotBindScript.Replace("#SCRIPTNAME#", className);
            tmpNotBindScript = tmpNotBindScript.Replace("#TIME#", DateTime.Now.ToString());

            if (!File.Exists($"{codePath}/{className}.cs"))
            {
                using (StreamWriter sw = new StreamWriter($"{codePath}/{className}.cs"))
                {
                    sw.Write(tmpNotBindScript);
                    sw.Close();
                }
            }
            AssetDatabase.Refresh();
        }

        


        /// <summary>
        /// 获取组件路径
        /// </summary>
        /// <param name="com">组件的transform</param>
        /// <returns></returns>
        private string GetComponentPath(Transform com)
        {
            if (com == com.root) return com.name;
            string result = "";
            Transform root = _autoBindTool.transform;
            while (com.parent != root)
            {
                result = "/" + com.name + result;
                com = com.parent;
            }
            result = com.name + result;
            return result;
        }
        /// <summary>
        /// 给当前物体添加新创建的目标类
        /// </summary>
        private void AddTargetClass()
        {
            GameObject obj = _autoBindTool.gameObject;
            string classFullName = "";
            if (!string.IsNullOrEmpty(_autoBindTool.NameSpace))
            {
                classFullName = _autoBindTool.NameSpace + "." + _autoBindTool.ClassName;
            }
            else
            {
                classFullName = _autoBindTool.ClassName;
            }

            Type targetClass = GetClass(classFullName, _assemblyNames);
            if (obj.GetComponent(targetClass) == null)
            {
                obj.AddComponent(targetClass);
            }
            //obj.BroadcastMessage("GetBindComponents", SendMessageOptions.DontRequireReceiver);
        }

        /// <summary>
        /// 生成绑定规则实例
        /// </summary>
        /// <typeparam name="T">IBindRule</typeparam>
        /// <param name="ruleName">规则名称</param>
        /// <param name="assemblyNames">程序集名称列表</param>
        /// <returns></returns>
        private IBindRule CreateRuleInstance(string ruleName, List<string> assemblyNames)
        {
            foreach (var item in assemblyNames)
            {
                Assembly assembly = Assembly.Load(item);

                IBindRule instance = (IBindRule)assembly.CreateInstance(ruleName);
                if (instance != null) return instance;
            }
            return default(IBindRule);
        }

        /// <summary>
        /// 从程序集中获取所有实现了IBindRule接口的类
        /// </summary>
        /// <param name="typeBase">接口</param>
        /// <param name="assemblyNames">程序集名称列表</param>
        /// <returns></returns>
        private string[] GetBindRules(List<string> assemblyNames)
        {
            Type typeBase = typeof(IBindRule);
            List<string> typeNames = new List<string>();

            foreach (var item in assemblyNames)
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(item);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
        private Type GetClass(string className, List<string> assemblyNames)
        {
            foreach (var item in assemblyNames)
            {
                Assembly assembly = Assembly.Load(item);
                Type targetClass = assembly.GetType(className);
                if (targetClass != null) return targetClass;
            }
            return null;
        }
    }
}
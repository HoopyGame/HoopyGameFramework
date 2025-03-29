
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：文件处理
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.IO;
using UnityEngine;

public static class FileUtils
{
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

    public static T LoadDataFile<T>(string fileName) where T: ScriptableObject
    {
        LoadFileFactory loadFileFactory = new LoadResourcesFile();
        return loadFileFactory.LoadScriptableObjectFile<T>(fileName);
    }
}

public abstract class LoadFileFactory
{
    public abstract T LoadScriptableObjectFile<T>(string fileName) where T : ScriptableObject;
}
public class LoadResourcesFile : LoadFileFactory
{
    public override T LoadScriptableObjectFile<T>(string fileName)
    {
        return Resources.Load<T>("Datas/" + fileName);
    }
}

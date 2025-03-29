/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：字符串处理工具
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
public static class StringUtils
{
    /// <summary>
    /// 检测路径是否有后缀，没有自动加上
    /// </summary>
    /// <param name="path"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static string CheckAndFixedExtension(this string path, string extension)
    {
        return path.EndsWith(extension) ? path : path + extension;
    }
}
/// <summary>
/// 存放所有文件的后缀名
/// </summary>
public class FileExtension
{
    public const string prefab = ".prefab";

    public const string dll = ".dll";
    public const string pdb = ".pdb";

    public const string wav = ".wav";
    public const string mp3 = ".mp3";

    public const string mp4 = ".mp4";

    public const string png = ".png";
    public const string jpg = ".jpg";

    public const string txt = ".txt";
}
public class DefaultVolumeField
{
    //比较舒服的节奏
    public const float OpenPopupTime = .45f;
    public const float ClosePopupTime = .35f;
}
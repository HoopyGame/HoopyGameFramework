/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：时间处理工具
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;

public static class TimeUtils
{
    #region 时间戳部分
    /// <summary>
    /// 获取当前时间戳（自1970年1月1日以来的秒数）
    /// </summary>
    /// <returns>转换成秒</returns>
    public static long GetCurrentTimeStampToSeconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    /// <summary>
    /// 获取指定时间戳对应的日期时间字符串 秒
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTimeOffset GetDateTimeOffectFromTimestampBySecondes(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp);
    }
    /// <summary>
    /// 获取当前时间戳（自1970年1月1日以来的毫秒数）
    /// </summary>
    /// <returns>转换成秒</returns>
    public static long GetCurrentTimeStampToMilliseconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    /// <summary>
    /// 获取指定时间戳对应的日期时间字符串 毫秒
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTimeOffset GetDateTimeOffectFromTimestampMillisecondes(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
    }
    /// <summary>
    /// 将时间转换成时间戳 秒
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ConvertTimeStampToSeconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
    /// <summary>
    /// 将时间转换成时间戳 毫秒
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ConvertTimeStampToMilliseconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }

    #endregion
    /// <summary>
    /// 获取当前的日期时间字符串（格式：yyyy-MM-dd HH:mm:ss）
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentDateTimeString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    /// <summary>
    /// 获取两个时间的时间间隔
    /// </summary>
    /// <param name="startTime">第一个时间</param>
    /// <param name="currentTime">第二个时间</param>
    /// <returns></returns>
    public static TimeSpan GetDateTimeInterval(DateTime startTime, DateTime currentTime)
    {
        return currentTime - startTime;
    }
}

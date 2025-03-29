/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：截图工具，支持URP，全屏截图和指定相机截图
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
//using UnityEngine.Rendering.Universal;

public static class PhotographUtils
{
    /// <summary>
    /// 自带API 直接截取当前屏幕并保存
    /// </summary>
    /// <param name="fileName"></param>
    public static void ScreenCpture(string fileName, UnityAction callBack = null)
    {
        ScreenCapture.CaptureScreenshot(fileName);
        callBack?.Invoke();
        DebugUtils.Print("截取了一张图片并保存了 名字:" + fileName);
    }
    /// <summary>
    /// 通过相机截取Rect大小的图
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="rect"></param>
    /// <param name="callBack"></param>
    /// <param name="textureForamat"></param>
    /// <returns></returns>
    public static async UniTask<Texture2D> PhotographByRect(this Camera camera, Rect rect,bool isURPBase = true, UnityAction callBack = null, TextureFormat textureForamat = TextureFormat.RGB24)
    {
        //UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
        //if (!isURPBase && universalAdditionalCameraData)
        //{
        //    universalAdditionalCameraData.renderType = CameraRenderType.Base;
        //}
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();
        await UniTask.Yield();
        //if (!isURPBase && universalAdditionalCameraData)
        //{
        //    universalAdditionalCameraData.renderType = CameraRenderType.Overlay;
        //}
        camera.targetTexture = null;
        RenderTexture.active = renderTexture;
        Texture2D screenCapture = new Texture2D((int)rect.width, (int)rect.height, textureForamat, false);
        screenCapture.ReadPixels(rect, 0, 0);
        screenCapture.Apply();
        RenderTexture.active = null;

        callBack?.Invoke();

        return screenCapture;
    }
}

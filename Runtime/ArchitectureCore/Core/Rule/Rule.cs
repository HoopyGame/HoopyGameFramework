/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：规则层，整套架构依赖此规则
│　创 建 人*：Hoopy
│　创建时间：2025-02-25 09:29:18
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/

using System;

namespace HoopyGame.Core
{
    public interface IBelongToHGArchitecture
    {
        IHGArchitecture GetHGArchitecture();
    }
    public interface ICanSetHGArchitecture
    {
        void SetHGArichitecture(IHGArchitecture hgArchitecture);
    }

    /// <summary>
    /// 可以获取Model
    /// </summary>
    public interface ICanGetModel : IBelongToHGArchitecture { }
    public static class GetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel =>
            self.GetHGArchitecture().GetModel<T>();
    }

    /// <summary>
    /// 可以获取Systme
    /// </summary>
    public interface ICanGetSystem : IBelongToHGArchitecture { }
    public static class GetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem =>
            self.GetHGArchitecture().GetSystem<T>();
    }

    /// <summary>
    /// 可以获取Utility
    /// </summary>
    public interface ICanGetUtility : IBelongToHGArchitecture { }
    public static class GetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility =>
            self.GetHGArchitecture().GetUtility<T>();
    }

    //可以发送命令
    public interface ICanSendCommand : IBelongToHGArchitecture { }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new() =>
            self.GetHGArchitecture().SendCommand<T>(new T());

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand =>
            self.GetHGArchitecture().SendCommand<T>(command);

        public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command) =>
            self.GetHGArchitecture().SendCommand(command);
    }

    //可以注册事件
    public interface ICanRegisterEvent : IBelongToHGArchitecture { }
    public static class CanRegisterEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) =>
            self.GetHGArchitecture().RegisterEvent(onEvent);
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent) =>
            self.GetHGArchitecture().UnRegisterEvent<T>(onEvent);
    }
    //可以发送事件
    public interface ICanSendEvent : IBelongToHGArchitecture { }
    public static class CanSendEventExtension
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new() =>
            self.GetHGArchitecture().SendEvent<T>();

        public static void SendEvent<T>(this ICanSendEvent self, T e) => self.GetHGArchitecture().SendEvent<T>(e);
    }
    //可以发送查询
    public interface ICanSendQuery : IBelongToHGArchitecture { }
    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query) =>
            self.GetHGArchitecture().SendQuery(query);
    }

    public interface ICanInit
    {
        bool Initialized { get; set; }
        void Init();
        void DeInit();
    }
}
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：类型事件
│　创 建 人*：Hoopy
│　创建时间：2025-02-27 13:53:39
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoopyGame.Core
{
    public interface IUnRegister
    {
        void UnRegister();
    }
    public interface IUnRegisterList
    {
        List<IUnRegister> UnRegisterList { get; }
    }
    /// <summary>
    /// 对UnregisterList的扩展方法
    /// </summary>
    public static class IUnRegisterListExtension
    {
        public static void AddToUnregisterList(this IUnRegister sefl, IUnRegisterList unregisterLsit)
            => unregisterLsit.UnRegisterList.Add(sefl);
        public static void UnRegisterAll(this IUnRegisterList sefl)
        {
            foreach (IUnRegister item in sefl.UnRegisterList)
            {
                item.UnRegister();
            }
            sefl.UnRegisterList.Clear();
        }
    }

    public struct CustomUnRegister : IUnRegister
    {
        private Action _onUnRegister { get; set; }

        public CustomUnRegister(Action onUnRegister) => _onUnRegister = onUnRegister;

        public void UnRegister()
        {
            _onUnRegister?.Invoke();
            _onUnRegister = null;
        }
    }

    public abstract class UnRegisterTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnRegister> _unRegisters = new ();

        public IUnRegister AddUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Add(unRegister);
            return unRegister;
        }
        public void RemoveUnRegister(IUnRegister unRegister) => _unRegisters.Remove(unRegister);

        public void UnRegister()
        {
            foreach (var unRegister in _unRegisters)
            {
                unRegister.UnRegister();
            }

            _unRegisters.Clear();
        }
    }

    public class UnRegisterOnDestoryTrigger : UnRegisterTrigger
    {
        private void OnDestroy()
        {
            UnRegister();
        }
    }
    public class UnRegisterOnDisableTrigger : UnRegisterTrigger
    {
        private void OnDisable()
        {
            UnRegister();
        }
    }

    #region UnRegisterExtension
   
    public static class UnRegisterExtension
    {
        static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            var trigger = gameObject.GetComponent<T>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<T>();
            }

            return trigger;
        }
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
            => GetOrAddComponent<UnRegisterOnDestoryTrigger>(gameObject)
            .AddUnRegister(unRegister);
        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component =>
            self.UnRegisterWhenGameObjectDestroyed(component.gameObject);

        public static IUnRegister UnRegisterWhenDisabled<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component =>
            self.UnRegisterWhenDisabled(component.gameObject);

        public static IUnRegister UnRegisterWhenDisabled(this IUnRegister unRegister,
            UnityEngine.GameObject gameObject) =>
            GetOrAddComponent<UnRegisterOnDisableTrigger>(gameObject)
                .AddUnRegister(unRegister);

        //public static IUnRegister UnRegisterWhenCurrentSceneUnloaded(this IUnRegister self) =>
        //    UnRegisterCurrentSceneUnloadedTrigger.Get.AddUnRegister(self);
    }
    #endregion

    public class TypeEventSystem
    {
        private readonly Events _events = new ();

        public static readonly TypeEventSystem Global = new ();

        public void Send<T>() where T : new() => _events.GetEvent<Event<T>>()?.Trigger(new T());

        public void Send<T>(T e) => _events.GetEvent<Event<T>>()?.Trigger(e);

        public IUnRegister Register<T>(Action<T> onEvent) => _events.GetOrAddEvent<Event<T>>().Register(onEvent);

        public void UnRegister<T>(Action<T> onEvent)
        {
            var e = _events.GetEvent<Event<T>>();
            e?.UnRegister(onEvent);
        }
    }
}
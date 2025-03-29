/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：事件层
│　创 建 人*：Hoopy
│　创建时间：2025-02-27 13:48:39
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;

namespace HoopyGame.Core
{
    public interface IEvent
    {
        IUnRegister Register(Action onEvent);
    }
    public class Event : IEvent
    {
        private Action _onEvent = () => { };
        public IUnRegister Register(Action onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }
        public void UnRegister(Action onEvent) => _onEvent -= onEvent;
        public void Trigger() => _onEvent?.Invoke();

    }
    public class Event<T> : IEvent
    {
        private Action<T> _onEvent = e => { };

        public IUnRegister Register(Action<T> onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T> onEvent) => _onEvent -= onEvent;

        public void Trigger(T t) => _onEvent?.Invoke(t);

        IUnRegister IEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent();
        }
    }
    public class Event<T, K> : IEvent
    {
        private Action<T, K> _onEvent = (t, k) => { };

        public IUnRegister Register(Action<T, K> onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K> onEvent) => _onEvent -= onEvent;

        public void Trigger(T t, K k) => _onEvent?.Invoke(t, k);

        IUnRegister IEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __) => onEvent();
        }
    }
    public class Event<T, K, S> : IEvent
    {
        private Action<T, K, S> _onEvent = (t, k, s) => { };

        public IUnRegister Register(Action<T, K, S> onEvent)
        {
            _onEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K, S> onEvent) => _onEvent -= onEvent;

        public void Trigger(T t, K k, S s) => _onEvent?.Invoke(t, k, s);

        IUnRegister IEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _, K __, S ___) => onEvent();
        }
    }
    public class Events
    {
        private static readonly Events _globalEvents = new();

        public static T Get<T>() where T : IEvent => _globalEvents.GetEvent<T>();

        public static void Register<T>() where T : IEvent, new() => _globalEvents.AddEvent<T>();

        private readonly Dictionary<Type, IEvent> mTypeEvents = new Dictionary<Type, IEvent>();

        public void AddEvent<T>() where T : IEvent, new() => mTypeEvents.Add(typeof(T), new T());

        public T GetEvent<T>() where T : IEvent
        {
            return mTypeEvents.TryGetValue(typeof(T), out var e) ? (T)e : default;
        }

        public T GetOrAddEvent<T>() where T : IEvent, new()
        {
            var eType = typeof(T);
            if (mTypeEvents.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = new T();
            mTypeEvents.Add(eType, t);
            return t;
        }
    }

    #region Event Extension

    public class OrEvent : IUnRegisterList
    {
        public OrEvent Or(IEvent easyEvent)
        {
            easyEvent.Register(Trigger).AddToUnregisterList(this);
            return this;
        }

        private Action mOnEvent = () => { };

        public IUnRegister Register(Action onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent)
        {
            mOnEvent -= onEvent;
            this.UnRegisterAll();
        }

        private void Trigger() => mOnEvent?.Invoke();

        public List<IUnRegister> UnRegisterList { get; } = new List<IUnRegister>();
    }

    public static class OrEventExtensions
    {
        public static OrEvent Or(this IEvent self, IEvent e) => new OrEvent().Or(self).Or(e);
    }

    #endregion
}
/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：数据+数据事件层
│　创 建 人*：Hoopy
│　创建时间：2025-02-28 11:45:51
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/


using System;

namespace HoopyGame.Core
{
    public interface IBindableProperty<T> : IReadonlyBindableProperty<T>
	{
		new T Value { get; }
        void SetValueWithoutEvent(T newValue);
    }
	public interface IReadonlyBindableProperty<T> : IEvent
	{
		T Value { get; }

        IUnRegister RegisterWithInitValue(Action<T> action);
        void UnRegister(Action<T> onValueChanged);
        IUnRegister Register(Action<T> onValueChanged);
    }

    public class BindableProperty<T> : IBindableProperty<T>
    {
        public BindableProperty(T defaultValue = default) => mValue = defaultValue;

        protected T mValue;

        public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

        public BindableProperty<T> WithComparer(Func<T, T, bool> comparer)
        {
            Comparer = comparer;
            return this;
        }

        public T Value
        {
            get => GetValue();
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && Comparer(value, mValue)) return;

                SetValue(value);
                mOnValueChanged.Trigger(value);
            }
        }

        protected virtual void SetValue(T newValue) => mValue = newValue;

        protected virtual T GetValue() => mValue;

        public void SetValueWithoutEvent(T newValue) => mValue = newValue;

        private Event<T> mOnValueChanged = new Event<T>();

        public IUnRegister Register(Action<T> onValueChanged)
        {
            return mOnValueChanged.Register(onValueChanged);
        }

        public IUnRegister RegisterWithInitValue(Action<T> onValueChanged)
        {
            onValueChanged(mValue);
            return Register(onValueChanged);
        }

        public void UnRegister(Action<T> onValueChanged) => mOnValueChanged.UnRegister(onValueChanged);

        IUnRegister IEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent();
        }

        public override string ToString() => Value.ToString();
    }
    internal class ComparerAutoRegister
    {
#if UNITY_5_6_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoRegister()
        {
            BindableProperty<int>.Comparer = (a, b) => a == b;
            BindableProperty<float>.Comparer = (a, b) => a == b;
            BindableProperty<double>.Comparer = (a, b) => a == b;
            BindableProperty<string>.Comparer = (a, b) => a == b;
            BindableProperty<long>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector4>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Color32>.Comparer =
                (a, b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
            BindableProperty<UnityEngine.Bounds>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Rect>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Quaternion>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector2Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.Vector3Int>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.BoundsInt>.Comparer = (a, b) => a == b;
            BindableProperty<UnityEngine.RangeInt>.Comparer = (a, b) => a.start == b.start && a.length == b.length;
            BindableProperty<UnityEngine.RectInt>.Comparer = (a, b) => a.Equals(b);
        }
#endif
    }
}